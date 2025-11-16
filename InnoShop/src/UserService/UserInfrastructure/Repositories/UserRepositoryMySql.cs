using Microsoft.EntityFrameworkCore;
using UserApplication.Interfaces;
using UserDomain.Models;
using UserInfrastructure.DataSources;
using UserInfrastructure.Entities;

namespace UserInfrastructure.Repositories;

public class UserRepositoryMySql : IUserRepository
{
    private readonly UserContextMySql userContext;

    public UserRepositoryMySql(UserContextMySql userContextMySql)
    {
        userContext = userContextMySql;
    }

    public async Task<List<User>> GetAllUsers()
    {
        var users = await userContext.Users.ToListAsync();
        return users.Select(u => ConvertUser(u)).ToList();
    }

    public async Task<User?> GetUserById(int id)
    {
        var user = await userContext.Users.FindAsync(id);
        return user != null ? ConvertUser(user) : null;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await userContext.Users.SingleOrDefaultAsync(u => u.Email == email);
        return user != null ? ConvertUser(user) : null;
    }

    public async Task<User> CreateUser(User user)
    {
        UserEntity userToInsert = ConvertUser(user);

        await userContext.Users.AddAsync(userToInsert);
        await userContext.SaveChangesAsync();

        return ConvertUser(userToInsert);
    }

    public async Task UpdateUser(User user)
    {
        var userEntity = await userContext.Users.FindAsync(user.Id);
        if (userEntity == null)
            return;

        userEntity.Name = user.Name;
        userEntity.Email = user.Email;
        userEntity.UserRole = user.UserRole;
        userEntity.IsActive = user.IsActive;

        await userContext.SaveChangesAsync();
    }

    public async Task SetUserActive(int id, bool active)
    {
        var userEntity = await userContext.Users.FindAsync(id);
        if (userEntity == null)
            return;

        userEntity.IsActive = active;

        await userContext.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
        var userEntity = await userContext.Users.FindAsync(id);
        if (userEntity == null)
            return;

        userContext.Users.Remove(userEntity);
        await userContext.SaveChangesAsync();
    }


    private User ConvertUser(UserEntity userEntity)
    {
        return new User
        {
            Id = userEntity.Id,
            Name = userEntity.Name,
            Email = userEntity.Email,
            UserRole = userEntity.UserRole,
            HashPassword = userEntity.HashPassword,
            IsActive = userEntity.IsActive,
        };
    }

    private UserEntity ConvertUser(User user)
    {
        return new UserEntity
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            UserRole = user.UserRole,
            HashPassword = user.HashPassword,
            IsActive = user.IsActive,
        };
    }

}