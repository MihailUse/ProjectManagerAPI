using API.Models.User;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class UserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.SingleAsync(x => x.Id == id);
        }

        public async Task<User> GetUserByLoginAsync(string login)
        {
            return await _context.Users.SingleAsync(x => x.Login == login);
        }

        public bool IsLoginExists(string login)
        {
            return _context.Users.Any(x => x.Login == login);
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.AsEnumerable();
        }

        public async Task<Guid> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<User> UpdateUserAsync(Guid id, User newUser)
        {
            User user = await GetUserByIdAsync(id);

            if (newUser.Login != null && !IsLoginExists(newUser.Login))
                user.Login = newUser.Login ?? user.Login;

            user.About = newUser.About ?? user.About;

            await _context.SaveChangesAsync();
            return user;
        }

        public async System.Threading.Tasks.Task DeleteUserAsync(Guid id)
        {
            User user = await GetUserByIdAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
