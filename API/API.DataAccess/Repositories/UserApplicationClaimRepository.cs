using API.DataAccess.Interfaces.Repositories;
using API.Domain;
using API.EFCore;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Repositories
{
    public class UserApplicationClaimRepository : IUserApplicationClaimRepository
    {
        private readonly IdentityServerDbContext _context;
        public UserApplicationClaimRepository(IdentityServerDbContext identityServerDbContext)
        {
            _context = identityServerDbContext;
        }
        public async Task<IEnumerable<UserApplicationClaim>> GetAllApplicationClaimsAsync(int userId, int applicationId)
        {
            return await _context.Set<UserApplicationClaim>()
              .Where(e => e.User.Id == userId && e.ApplicationClaim.Application.Id == applicationId)
              .Select(e => new UserApplicationClaim
              {
                  Id = e.Id,
                  ApplicationClaim = e.ApplicationClaim,
                  Value = e.Value
              })
              .ToArrayAsync();
        }

        public async Task<IEnumerable<UserApplicationClaim>> GetApplicationClaimsOfAsync(int userId)
        {
            return await _context.Set<UserApplicationClaim>()
                    .Where(u => u.User.Id == userId)
                    .ToListAsync();
        }

        public void RemoveRange(IEnumerable<int> ids)
        {
            var userApplicationClaims = ids.Select(id => new UserApplicationClaim
            {
                Id = id
            });

            _context.Set<UserApplicationClaim>()
                .RemoveRange(userApplicationClaims);
        }
    }
}
