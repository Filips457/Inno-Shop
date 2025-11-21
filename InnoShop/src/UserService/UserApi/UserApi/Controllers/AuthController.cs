using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UserApplication.Interfaces;

namespace UserApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService service;
    private readonly IJwtTokenGenerator tokenGenerator;

    public AuthController(IUserService userService, IJwtTokenGenerator jwtTokenGenerator)
    {
        service = userService;
        tokenGenerator = jwtTokenGenerator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await service.ValidateCredentials(request.Email, request.Password);

        var token = tokenGenerator.GenerateToken(user);
        return Ok(new { Token = token });
    }
}