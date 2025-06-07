namespace Dor.Dtos.Building;

public class BuildingDto
{
    public int Id { get; set; }
    public string? BuildingName { get; set; }
    public int NumberOfFloors { get; set; }
    public List<string>? Services { get; set; }
    public string? Status { get; set; }
    public string? MapPath { get; set; }
}


