using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dor.Models;

public class Building
{
    [Key]
    public int Id { get; set; }

    [StringLength(100, ErrorMessage = "Building name cannot exceed 100 characters")]
    public string? BuildingName { get; set; }

    [Range(1, 300, ErrorMessage = "Number of floors must be between 1 and 300")]
    public int NumberOfFloors { get; set; }

    public List<string>? Services { get; set; } = new List<string>();

    public string? Status { get; set; }

    public string? MapPath { get; set; }
}