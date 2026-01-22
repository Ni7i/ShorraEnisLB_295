namespace ShorraEnisLB_295.Models;

using System.ComponentModel.DataAnnotations;

public class TeaRecipe
{
    public int Id { get; set; }

    [Required(ErrorMessage = "TeaName ist ein Pflichtfeld")]
    [MaxLength(100)]
    public string TeaName { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Ingredients { get; set; }

    [Required(ErrorMessage = "SteepingTimeInMinutes ist ein Pflichtfeld")]
    [Range(1, 60, ErrorMessage = "Ziehzeit muss zwischen 1 und 60 Minuten liegen")]
    public int SteepingTimeInMinutes { get; set; }

    [Range(0, 100, ErrorMessage = "Temperatur muss zwischen 0 und 100°C liegen")]
    public int? TemperatureCelsius { get; set; }

    [Required]
    public int CollectionId { get; set; }

    public Collection? Collection { get; set; }
}