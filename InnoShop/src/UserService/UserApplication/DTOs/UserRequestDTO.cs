using System.ComponentModel.DataAnnotations;
using UserDomain.Models;

namespace UserApplication.DTOs;

public class UserRequestDTO
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; }

    public Role UserRole { get; set; }

    public bool IsActive { get; set; }
}