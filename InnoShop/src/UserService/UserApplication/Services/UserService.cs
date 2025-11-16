using System.Text.RegularExpressions;
using UserApplication.DTOs;
using UserApplication.Interfaces;
using UserDomain.Models;

namespace UserApplication.Services;

public class UserService : IUserService
{
    private readonly IUserRepository repository;
    private readonly IPasswordHasher passwordHasher;

    private readonly Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public UserService(IUserRepository userRepository, IPasswordHasher hasher)
    {
        repository = userRepository;
        passwordHasher = hasher;
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

    public async Task<UserDTO> ValidateCredentials(string email, string password)
    {
        var user = await repository.GetUserByEmail(email);

        if (user == null)
            throw new Exception($"User with email {email} was not found.");

        if (user.IsActive == false)
            throw new Exception($"User with email {email} is inactive.");

        if (passwordHasher.Verify(user.HashPassword, password) == false)
            throw new Exception($"Incorrect password!");

        return ConvertUser(user);
    }

    public async Task<UserDTO> CreateUser(UserRequestDTO userRequestDTO)
    {
        if (emailRegex.IsMatch(userRequestDTO.Email) == false)
            throw new Exception("Incorrect email!");

        var user = ConvertUser(userRequestDTO);
        user.Id = 0;
        await repository.CreateUser(user);

        return ConvertUser(user);
    }

    public async Task UpdateUser(int id, UserRequestDTO userRequestDTO)
    {
        if (emailRegex.IsMatch(userRequestDTO.Email) == false)
            throw new Exception("Incorrect email!");

        var user = await repository.GetUserById(id);
        if (user == null)
            throw new Exception($"User with id {id} was not found.");

        user.Name = userRequestDTO.Name;
        user.Email = userRequestDTO.Email;
        user.UserRole = userRequestDTO.UserRole;
        user.HashPassword = passwordHasher.Hash(userRequestDTO.Password);
        user.IsActive = userRequestDTO.IsActive;

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

    private User ConvertUser(UserRequestDTO userRequestDTO)
    {
        return new User
        {
            Name = userRequestDTO.Name,
            Email = userRequestDTO.Email,
            UserRole = userRequestDTO.UserRole,
            HashPassword = passwordHasher.Hash(userRequestDTO.Password),
            IsActive = userRequestDTO.IsActive,
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