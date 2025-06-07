using System.ComponentModel.DataAnnotations;

namespace Dor.Models;

public class Complex
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Complex Name")]
    [StringLength(100, ErrorMessage = "Complex name cannot exceed 100 characters")]
    public string? ComplexName { get; set; }

    [StringLength(50)]
    public string? Type { get; set; }

    [Display(Name = "Logo Path")]
    public string? LogoPath { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }
}