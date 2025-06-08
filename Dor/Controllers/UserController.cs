using Dor.Dtos;
using Dor.Interfaces;
using Dor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dor.Controllers;

[ApiController]
[Route("user")]
[AllowAnonymous]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IRepository<User> _userRepository;

    public UserController(IAuthService authService, IRepository<User> userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    [HttpPost("auth")]
    public async Task<ActionResult<AuthenticationResponse>> AuthenticateUser(Dtos.AuthenticationRequest request)
    {
        var result = await _authService.AuthenticateAsync(request.Name, request.Password);
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

    [HttpPost("create")]
    public async Task<ActionResult<int>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        if (string.IsNullOrWhiteSpace(createUserDto.Name) || string.IsNullOrWhiteSpace(createUserDto.Password))
            return BadRequest("Name and Password are required.");

        var newUser = new User
        {
            Name = createUserDto.Name,
            Password = createUserDto.Password, // Consider hashing the password
            Role = createUserDto.Role ?? "User"
        };

        try
        {
            await _userRepository.AddAsync(newUser);
            return Ok(newUser.Id);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}