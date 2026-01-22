using System.ComponentModel.DataAnnotations;

namespace ShorraEnisLB_295.Models;

public class RefreshRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}