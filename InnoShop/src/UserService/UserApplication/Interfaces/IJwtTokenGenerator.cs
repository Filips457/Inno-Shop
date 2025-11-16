using UserApplication.DTOs;

namespace UserApplication.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(UserDTO user);
}