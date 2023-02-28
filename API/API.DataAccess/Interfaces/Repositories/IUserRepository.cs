using API.Domain;

namespace API.DataAccess.Interfaces.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserAsync(int id);
        void DeleteUserAsync(int id);
        Task<User> ModifyUserAsync(User user);
    }
}
