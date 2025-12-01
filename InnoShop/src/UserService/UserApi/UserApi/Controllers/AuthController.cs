using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UserApplication.Interfaces;

namespace UserApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService service;

    public AuthController(IUserService userService)
    {
        service = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await service.ValidateCredentials(request.Email, request.Password);

        var token = await service.GenerateJwtToken(user);
        return Ok(new { Token = token });
    }
}