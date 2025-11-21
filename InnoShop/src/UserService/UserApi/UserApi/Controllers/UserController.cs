using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApplication.DTOs;
using UserApplication.Interfaces;
using UserDomain.Models;

namespace UserApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService service;

    public UserController(IUserService userService)
    {
        service = userService;
    }

    [HttpGet("All-users")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
    {
        return Ok(await service.GetAllUsers());
    }

    [HttpGet("Active-users")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetActiveUsers()
    {
        return Ok(await service.GetActiveUsers());
    }

    [HttpGet("User-by-id")]
    public async Task<ActionResult<UserDTO>> GetUserById([FromQuery] int id)
    {
        return await service.GetUserById(id);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO userRequestDTO)
    {
        var user = await service.CreateUser(userRequestDTO);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromQuery] int id, [FromBody] UserRequestDTO userRequestDTO)
    {
        await service.UpdateUser(id, userRequestDTO);
        return NoContent();
    }

    [Authorize(Roles = nameof(Role.Admin))]
    [HttpPut("set-active")]
    public async Task<IActionResult> SetUserActive([FromQuery] int id, [FromQuery] bool active)
    {
        await service.SetUserActive(id, active);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromQuery] int id)
    {
        await service.DeleteUser(id);
        return NoContent();
    }
}