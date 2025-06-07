using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dor.Models;

public class Building
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Building Name")]
    [StringLength(100, ErrorMessage = "Building name cannot exceed 100 characters")]
    public string? BuildingName { get; set; }

    [Display(Name = "Number of Floors")]
    [Range(1, 300, ErrorMessage = "Number of floors must be between 1 and 300")]
    public int NumberOfFloors { get; set; }

    public List<string>? Services { get; set; }

    [StringLength(50)]
    public string? Status { get; set; }

    [Display(Name = "Map Path")]
    public string? MapPath { get; set; }
}