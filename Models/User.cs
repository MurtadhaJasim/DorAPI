namespace Dor.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = "User";

    // Refresh Token Support
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}
