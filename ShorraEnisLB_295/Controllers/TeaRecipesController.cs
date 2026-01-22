namespace ShorraEnisLB_295.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShorraEnisLB_295.Data;
using ShorraEnisLB_295.Models;

[Route("api")]
[ApiController]
public class TeaRecipesController : ControllerBase
{
    private readonly TeaDbContext _context;

    public TeaRecipesController(TeaDbContext context)
    {
        _context = context;
    }

    // GET: api/collections/5/recipes
    [HttpGet("collections/{collectionId}/recipes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<TeaRecipe>>> GetRecipesByCollection(int collectionId)
    {
        var collectionExists = await _context.Collections.AnyAsync(c => c.Id == collectionId);
        
        if (!collectionExists)
        {
            return NotFound(new { message = $"Collection mit ID {collectionId} wurde nicht gefunden" });
        }

        var recipes = await _context.TeaRecipes
            .Where(r => r.CollectionId == collectionId)
            .ToListAsync();

        return Ok(recipes);
    }

    // GET: api/recipes/5
    [HttpGet("recipes/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeaRecipe>> GetTeaRecipe(int id)
    {
        var recipe = await _context.TeaRecipes.FindAsync(id);

        if (recipe == null)
        {
            return NotFound(new { message = $"TeaRecipe mit ID {id} wurde nicht gefunden" });
        }

        return Ok(recipe);
    }

    // POST: api/collections/5/recipes
    [HttpPost("collections/{collectionId}/recipes")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeaRecipe>> PostTeaRecipe(int collectionId, TeaRecipe recipe)
    {
        var collectionExists = await _context.Collections.AnyAsync(c => c.Id == collectionId);
        
        if (!collectionExists)
        {
            return NotFound(new { message = $"Collection mit ID {collectionId} wurde nicht gefunden" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        recipe.CollectionId = collectionId;
        _context.TeaRecipes.Add(recipe);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTeaRecipe), new { id = recipe.Id }, recipe);
    }

    // PUT: api/recipes/5
    [HttpPut("recipes/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutTeaRecipe(int id, TeaRecipe recipe)
    {
        if (id != recipe.Id)
        {
            return BadRequest(new { message = "ID in URL stimmt nicht mit Recipe-ID überein" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var collectionExists = await _context.Collections.AnyAsync(c => c.Id == recipe.CollectionId);
        if (!collectionExists)
        {
            return BadRequest(new { message = $"Collection mit ID {recipe.CollectionId} existiert nicht" });
        }

        _context.Entry(recipe).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await TeaRecipeExists(id))
            {
                return NotFound(new { message = $"TeaRecipe mit ID {id} wurde nicht gefunden" });
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/recipes/5
    [HttpDelete("recipes/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTeaRecipe(int id)
    {
        var recipe = await _context.TeaRecipes.FindAsync(id);
        
        if (recipe == null)
        {
            return NotFound(new { message = $"TeaRecipe mit ID {id} wurde nicht gefunden" });
        }

        _context.TeaRecipes.Remove(recipe);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> TeaRecipeExists(int id)
    {
        return await _context.TeaRecipes.AnyAsync(e => e.Id == id);
    }
}