using UserApplication.DTOs;

namespace UserApplication.Interfaces;

public interface IUserService
{
    Task<List<UserDTO>> GetAllUsers();

    Task<List<UserDTO>> GetActiveUsers();

    Task<UserDTO> ValidateCredentials(string email, string password);

    Task<UserDTO> GetUserById(int id);

    Task<UserDTO> CreateUser(UserRequestDTO userRequestDTO);

    Task UpdateUser(int id, UserRequestDTO userRequestDTO);

    Task SetUserActive(int id, bool active);

    Task DeleteUser(int id);
}