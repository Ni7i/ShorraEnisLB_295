namespace ShorraEnisLB_295.Models;

using System.ComponentModel.DataAnnotations;

public class Collection
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name ist erforderlich")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? OriginRegion { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public ICollection<TeaRecipe> TeaRecipes { get; set; } = new List<TeaRecipe>();
}