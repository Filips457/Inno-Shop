using UserDomain.Models;

namespace UserApplication.DTOs;

public class UserDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public Role UserRole { get; set; }

    public bool IsActive { get; set; }
}