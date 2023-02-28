using API.DataAccess.Interfaces.Repositories;
using API.Domain;
using API.EFCore;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Repositories
{
    public class UserRefreshTokenRepository : IUserRefreshTokenRepository
    {
        private readonly IdentityServerDbContext _context;
        public UserRefreshTokenRepository(IdentityServerDbContext identityServerDbContext)
        {
            _context = identityServerDbContext;
        }

        public async Task AddUserRefreshTokenAsync(int userId, int applicationId, string refreshToken, long expiration)
        {
            await _context.Set<UserAppRefreshToken>().AddAsync(new UserAppRefreshToken
            {
                UserId = userId,
                ApplicationId = applicationId,
                RefreshToken = refreshToken,
                Expiration = expiration
            });
        }

        public async Task<UserAppRefreshToken?> GetUserRefreshTokenAsync(string refreshToken)
        {
            return await _context.Set<UserAppRefreshToken>().AsNoTracking()
                .FirstOrDefaultAsync(r => r.RefreshToken == refreshToken);
        }

        public async Task DeleteUserRefreshTokenAsync(int id)
        {
            _context.Remove<UserAppRefreshToken>(new UserAppRefreshToken { Id = id });
        }
    }
}
