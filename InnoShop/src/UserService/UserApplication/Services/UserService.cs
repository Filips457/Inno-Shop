using System.Text.RegularExpressions;
using UserApplication.DTOs;
using UserApplication.Interfaces;
using UserDomain.Models;

namespace UserApplication.Services;

public class UserService : IUserService
{
    private readonly IUserRepository repository;
    private readonly Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public UserService(IUserRepository userRepository)
    {
        repository = userRepository;
    }

    public async Task<List<UserDTO>> GetAllUsers()
    {
        var users = await repository.GetAllUsers();
        return users.Select(u => ConvertUser(u)).ToList();
    }

    public async Task<List<UserDTO>> GetActiveUsers()
    {
        var users = await repository.GetAllUsers();
        return users.Where(u => u.IsActive == true).Select(u => ConvertUser(u)).ToList();
    }

    public async Task<UserDTO> GetUserById(int id)
    {
        var user = await repository.GetUserById(id);

        if (user == null)
            throw new Exception($"User with id {id} was not found.");

        return ConvertUser(user);
    }

    public async Task<UserDTO> CreateUser(UserDTO userDTO)
    {
        if (emailRegex.IsMatch(userDTO.Email) == false)
            throw new Exception("Incorrect email!");

        userDTO.Id = 0;
        var user = await repository.CreateUser(ConvertUser(userDTO));

        return ConvertUser(user);
    }

    public async Task UpdateUser(int id, UserDTO userDTO)
    {
        if (emailRegex.IsMatch(userDTO.Email) == false)
            throw new Exception("Incorrect email!");

        var user = await repository.GetUserById(id);
        if (user == null)
            throw new Exception($"User with id {id} was not found.");

        user.Name = userDTO.Name;
        user.Email = userDTO.Email;
        user.UserRole = userDTO.UserRole;
        user.IsActive = userDTO.IsActive;

        await repository.UpdateUser(user);
    }

    public async Task SetUserActive(int id, bool active)
    {
        var user = await repository.GetUserById(id);
        if (user == null)
            throw new Exception($"User with id {id} was not found.");

        await repository.SetUserActive(id, active);
    }

    public async Task DeleteUser(int id)
    {
        var user = await repository.GetUserById(id);
        if (user == null)
            throw new Exception($"User with id {id} was not found.");

        await repository.DeleteUser(id);
    }


    private User ConvertUser(UserDTO userDTO)
    {
        return new User
        {
            Id = userDTO.Id,
            Name = userDTO.Name,
            Email = userDTO.Email,
            UserRole = userDTO.UserRole,
            IsActive = userDTO.IsActive,
        };
    }

    private UserDTO ConvertUser(User user)
    {
        return new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            UserRole = user.UserRole,
            IsActive = user.IsActive,
        };
    }
}