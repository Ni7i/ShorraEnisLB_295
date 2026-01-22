namespace ShorraEnisLB_295.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShorraEnisLB_295.Data;
using ShorraEnisLB_295.Models;

[Route("api/[controller]")]
[ApiController]
public class CollectionsController : ControllerBase
{
    private readonly TeaDbContext _context;

    public CollectionsController(TeaDbContext context)
    {
        _context = context;
    }

    // GET: api/collections
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Collection>>> GetCollections()
    {
        return Ok(await _context.Collections.ToListAsync());
    }

    // GET: api/collections/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Collection>> GetCollection(int id)
    {
        var collection = await _context.Collections.FindAsync(id);

        if (collection == null)
        {
            return NotFound(new { message = $"Collection mit ID {id} wurde nicht gefunden" });
        }

        return Ok(collection);
    }

    // POST: api/collections
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Collection>> PostCollection(Collection collection)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Collections.Add(collection);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCollection), new { id = collection.Id }, collection);
    }

    // PUT: api/collections/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutCollection(int id, Collection collection)
    {
        if (id != collection.Id)
        {
            return BadRequest(new { message = "ID in URL stimmt nicht mit Collection-ID überein" });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Entry(collection).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CollectionExists(id))
            {
                return NotFound(new { message = $"Collection mit ID {id} wurde nicht gefunden" });
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/collections/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCollection(int id)
    {
        var collection = await _context.Collections.FindAsync(id);
        
        if (collection == null)
        {
            return NotFound(new { message = $"Collection mit ID {id} wurde nicht gefunden" });
        }

        _context.Collections.Remove(collection);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> CollectionExists(int id)
    {
        return await _context.Collections.AnyAsync(e => e.Id == id);
    }
}