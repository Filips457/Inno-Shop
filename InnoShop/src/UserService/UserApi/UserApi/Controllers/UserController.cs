using Microsoft.AspNetCore.Mvc;
using UserApplication.DTOs;
using UserApplication.Interfaces;

namespace UserApi.Controllers;

//TODO: убрать много try/catch

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
        try
        {
            return Ok(await service.GetAllUsers());
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("Active-users")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetActiveUsers()
    {
        try
        {
            return Ok(await service.GetActiveUsers());
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("User-by-id")]
    public async Task<ActionResult<UserDTO>> GetUserById([FromQuery] int id)
    {
        try
        {
            return await service.GetUserById(id);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserRequestDTO userRequestDTO)
    {
        try
        {
            var user = await service.CreateUser(userRequestDTO);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromQuery] int id, [FromBody] UserRequestDTO userRequestDTO)
    {
        try
        {
            await service.UpdateUser(id, userRequestDTO);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message.Contains("not found"))
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("set-active")]
    public async Task<IActionResult> SetUserActive([FromQuery] int id, [FromQuery] bool active)
    {
        try
        {
            await service.SetUserActive(id, active);
            return NoContent();
        }
        catch (Exception ex) when (ex.Message.Contains("not found"))
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromQuery] int id)
    {
        try
        {
            await service.DeleteUser(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}