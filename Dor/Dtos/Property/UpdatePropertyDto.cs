namespace Dor.Dtos.Property;

public class UpdatePropertyDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Owner { get; set; }
    public string? PropertyType { get; set; }
    public string? Status { get; set; }
}