namespace UserDomain.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Role UserRole { get; set; }
    public bool IsActive { get; set; }
}