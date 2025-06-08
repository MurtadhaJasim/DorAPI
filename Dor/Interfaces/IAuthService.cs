using Dor.Dtos;

namespace Dor.Interfaces;

public interface IAuthService
{
    Task<AuthenticationResponse?> AuthenticateAsync(string username, string password);
    Task<AuthenticationResponse?> RefreshTokenAsync(string accessToken, string refreshToken);
}
