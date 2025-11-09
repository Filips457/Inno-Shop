using UserApplication.DTOs;

namespace UserApplication.Interfaces;

public interface IUserService
{
    Task<List<UserDTO>> GetAllUsers();

    Task<List<UserDTO>> GetActiveUsers();

    Task<UserDTO> GetUserById(int id);

    Task<UserDTO> CreateUser(UserDTO userDTO);

    Task UpdateUser(int id, UserDTO userDTO);

    Task SetUserActive(int id, bool active);

    Task DeleteUser(int id);
}