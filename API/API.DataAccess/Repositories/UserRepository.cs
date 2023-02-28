using API.DataAccess.Interfaces.Repositories;
using API.Domain;
using API.EFCore;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityServerDbContext _context;
        public UserRepository(IdentityServerDbContext identityServerDbContext)
        {
            _context = identityServerDbContext;
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Set<User>()
                .AddAsync(user);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Set<User>()
                .ToListAsync();
        }

        public void DeleteUserAsync(int id)
        {
            _context.Set<User>().Remove(new User { Id = id });
        }

        public async Task<User> GetUserAsync(int id) =>
            await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User> ModifyUserAsync(User user)
        {
            var userDb = await _context.Set<User>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == user.Id);

            if (userDb != null)
            {
                user.Password = userDb.Password;
                _context.Set<User>().Update(user);
                return user;
            }
            return null;
        }
    }
}
