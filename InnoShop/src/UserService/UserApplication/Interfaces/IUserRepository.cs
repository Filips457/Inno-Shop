using UserDomain.Models;

namespace UserApplication.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllUsers();

    Task<User?> GetUserById(int id);

    Task<User?> GetUserByEmail(string email);

    Task<User> CreateUser(User user);

    Task UpdateUser(User user);

    Task SetUserActive(int id, bool active);

    Task DeleteUser(int id);
}