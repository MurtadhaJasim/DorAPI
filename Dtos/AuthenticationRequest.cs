namespace Dor.Dtos;

public class AuthenticationRequest
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}
