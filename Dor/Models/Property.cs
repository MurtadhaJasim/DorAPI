using System.ComponentModel.DataAnnotations;

namespace Dor.Models;

public class Property
{
    [Key]
    public int Id { get; set; }

    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string? Name { get; set; }

    [StringLength(100)]
    public string? Owner { get; set; }

    public string? MapPath { get; set; }

    [StringLength(50)]
    public string? PropertyType { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }
}