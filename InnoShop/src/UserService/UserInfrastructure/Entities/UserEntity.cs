using System.ComponentModel.DataAnnotations;
using UserDomain.Models;

namespace UserInfrastructure.Entities;

public class UserEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public Role UserRole { get; set; }

    public bool IsActive { get; set; }
}