namespace Dor.Dtos;

public class CreateUserDto
{
    public string Name { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string? Role { get; set; }
}