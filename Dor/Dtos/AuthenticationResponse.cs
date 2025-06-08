namespace Dor.Dtos;

public class AuthenticationResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
