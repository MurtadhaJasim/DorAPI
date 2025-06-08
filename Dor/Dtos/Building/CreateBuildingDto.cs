namespace Dor.Dtos.Building;
public class CreateBuildingDto
{
    public string? BuildingName { get; set; }
    public int NumberOfFloors { get; set; }
    public List<string>? Services { get; set; }
    public string? Status { get; set; }
}