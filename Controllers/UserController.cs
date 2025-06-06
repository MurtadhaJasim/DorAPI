using Dor.Dtos;
using Dor.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dor.Controllers;

[ApiController]
[Route("user")]
[AllowAnonymous]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;

    public UserController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("auth")]
    public async Task<ActionResult<AuthenticationResponse>> AuthenticateUser(AuthenticationRequest request)
    {
        var result = await _authService.AuthenticateAsync(request.UserName, request.Password);
        if (result == null)
            return Unauthorized();

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthenticationResponse>> RefreshToken(RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);
        if (result == null)
            return Unauthorized();

        return Ok(result);
    }
}