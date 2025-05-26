using Microsoft.AspNetCore.Mvc;
using Pharmacy_Management_System.Services;
using Pharmacy_Management_System.Models.DTOs;



[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto) => Ok(await _authService.RegisterAsync(dto));

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return result.StartsWith("Invalid") ? Unauthorized(result) : Ok(new { token = result });
    }
}
