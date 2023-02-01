using Common;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Services;

public class UserService
{
    private readonly DataContext _context;

    public UserService(DataContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserById(Guid id)
    {
        return await _context.Users.SingleAsync(x => x.Id == id);
    }

    public async Task<User> GetUserByLogin(string login)
    {
        return await _context.Users.SingleAsync(x => x.Login == login);
    }

    public bool IsLoginExists(string login)
    {
        return _context.Users.Any(x => x.Login == login);
    }

    public IQueryable<User> GetUsers()
    {
        return _context.Users.AsNoTracking();
    }

    public async Task<Guid> CreateUser(User user)
    {
        if (IsLoginExists(user.Login))
            throw new Exception("User already exists");

        var entityEntry = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return entityEntry.Entity.Id;
    }

    public async Task<User> UpdateUser(Guid id, User userOptions)
    {
        var user = await GetUserById(id);

        if (userOptions.Login != null && _context.Users.Any(x => (x.Login == userOptions.Login) && (x.Id != user.Id)))
            user.Login = userOptions.Login;

        user.About = userOptions.About ?? user.About;

        await _context.SaveChangesAsync();
        return user;
    }

    public async System.Threading.Tasks.Task DeleteUser(Guid id)
    {
        var user = await GetUserById(id);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUserByCredential(string login, string password)
    {
        var user = await GetUserByLogin(login);

        if (!HashHelper.Verify(user.PasswordHash, password))
            throw new Exception("Password is incorrect");

        return user;
    }
}