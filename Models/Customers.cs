namespace Dor.Models;

public class Customers
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? PhotoPath { get; set; }
    public string? Marital_status { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public string? Job { get; set; }
    public string? Complex { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string? HouseNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; } = null;

}
