using System.Net.Sockets;
using API.DataAccess.Interfaces.Repositories;
using API.Domain;
using API.EFCore;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly IdentityServerDbContext _context;
        public ApplicationRepository(IdentityServerDbContext identityServerDbContext)
        {
            _context = identityServerDbContext;
        }

        public async Task CreateApplicationAsync(Application application)
        {
            await _context.Set<Application>()
                .AddAsync(application);
        }

        public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
        {
            return await _context.Set<Application>()
                .ToListAsync();
        }

        public async Task<Application?> GetApplicationAsync(string secret)
        {
            return await _context.Set<Application>()
                .FirstOrDefaultAsync(e => e.Secret == secret);
        }

        public async Task DeleteApplicationAsync(Application application)
        {
            if (application is null)
                throw new ArgumentNullException("application");

            _context.Set<Application>().Remove(application);
            await _context.SaveChangesAsync();
        }

        public async Task<Application?> GetApplicationAsync(int id)
        {
            return await _context.Set<Application>()
                .FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
