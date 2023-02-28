using API.DataAccess.Interfaces.Repositories;
using API.Domain;
using API.EFCore;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Repositories
{
    public class ApplicationClaimRepository : IApplicationClaimRepository
    {
        private readonly IdentityServerDbContext _context;
        public ApplicationClaimRepository(IdentityServerDbContext identityServerDbContext)
        {
            _context = identityServerDbContext;
        }

        public async Task<IEnumerable<ApplicationClaim>> GetApplicationClaimsAsync(int applicationId)
        {
            return await _context.Set<ApplicationClaim>()
                .Where(e => e.Application.Id == applicationId)
                .ToListAsync();
        }
    }
}
