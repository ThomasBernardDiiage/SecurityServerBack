using API.Domain;

namespace API.DataAccess.Interfaces.Repositories
{
    public interface IApplicationClaimRepository : IRepository
    {
        Task<IEnumerable<ApplicationClaim>> GetApplicationClaimsAsync(int applicationId);
    }
}
