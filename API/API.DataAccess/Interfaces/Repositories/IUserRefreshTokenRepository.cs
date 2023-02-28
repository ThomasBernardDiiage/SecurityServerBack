using API.Domain;

namespace API.DataAccess.Interfaces.Repositories
{
    public interface IUserRefreshTokenRepository : IRepository
    {
        Task AddUserRefreshTokenAsync(int userId, int applicationId, string refreshToken, long expiration);

        Task<UserAppRefreshToken?> GetUserRefreshTokenAsync(string refreshToken);

        Task DeleteUserRefreshTokenAsync(int id);
    }
}
