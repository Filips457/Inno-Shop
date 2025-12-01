using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using UserApi.Exceptions;
using UserApplication.DTOs;
using UserApplication.Interfaces;
using UserDomain.Models;

namespace UserApplication.Services;

public class UserService : IUserService
{
    private readonly IUserRepository repository;
    private readonly IPasswordHasher passwordHasher;
    private readonly HttpClient httpClient;
    private readonly IJwtTokenGenerator tokenGenerator;

    private readonly Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public UserService(IUserRepository userRepository, IPasswordHasher hasher, 
                       IHttpClientFactory httpClientFactory)
    {
        repository = userRepository;
        passwordHasher = hasher;

        httpClient = httpClientFactory.CreateClient("UserServiceClient");
    }

    public async Task<List<UserDTO>> GetAllUsers()
    {
        var users = await repository.GetAllUsers();
        return users.Select(u => ConvertUser(u)).ToList();
    }

    public async Task<List<UserDTO>> GetActiveUsers()
    {
        var users = await repository.GetActiveUsers();
        return users.Select(u => ConvertUser(u)).ToList();
    }

    public async Task<UserDTO> GetUserById(int id)
    {
        var user = await repository.GetUserById(id);

        if (user == null)
            throw new NotFoundException($"User with id {id} was not found.");

        return ConvertUser(user);
    }

    public async Task<UserDTO> ValidateCredentials(string email, string password)
    {
        var user = await repository.GetUserByEmail(email);

        if (user == null)
            throw new NotFoundException($"User with email {email} was not found.");

        if (user.IsActive == false)
            throw new ArgumentException($"User with email {email} is inactive.");

        if (passwordHasher.Verify(user.HashPassword, password) == false)
            throw new ArgumentException($"Incorrect password!");

        return ConvertUser(user);
    }

    public Task<string> GenerateJwtToken(UserDTO userDTO)
    {
        var token = tokenGenerator.GenerateToken(userDTO);

        return Task.FromResult(token);
    }

    public async Task<UserDTO> CreateUser(UserRequestDTO userRequestDTO)
    {
        if (emailRegex.IsMatch(userRequestDTO.Email) == false)
            throw new ArgumentException("Incorrect email!");

        var user = ConvertUser(userRequestDTO);
        user.Id = 0;
        await repository.CreateUser(user);

        return ConvertUser(user);
    }

    public async Task UpdateUser(int id, UserRequestDTO userRequestDTO)
    {
        if (emailRegex.IsMatch(userRequestDTO.Email) == false)
            throw new ArgumentException("Incorrect email!");

        var user = await repository.GetUserById(id);
        if (user == null)
            throw new NotFoundException($"User with id {id} was not found.");

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
            throw new NotFoundException($"User with id {id} was not found.");

        var res = await httpClient.PutAsync($"set-active/{id}/{active}", null);

        await repository.SetUserActive(id, active);
    }

    public async Task DeleteUser(int id)
    {
        var user = await repository.GetUserById(id);
        if (user == null)
            throw new NotFoundException($"User with id {id} was not found.");

        await repository.DeleteUser(id);
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