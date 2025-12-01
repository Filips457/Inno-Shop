using UserDomain.Models;

namespace UserApplication.DTOs;

public class UserRequestDTO
{
    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public Role UserRole { get; set; }

    public bool IsActive { get; set; }
}