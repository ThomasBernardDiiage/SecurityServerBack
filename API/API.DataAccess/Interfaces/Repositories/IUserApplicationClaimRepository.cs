using API.Domain;

namespace API.DataAccess.Interfaces.Repositories
{
    public interface IUserApplicationClaimRepository : IRepository
    {
        Task<IEnumerable<UserApplicationClaim>> GetAllApplicationClaimsAsync(int userId, int applicationId);
        Task<IEnumerable<UserApplicationClaim>> GetApplicationClaimsOfAsync(int userId);

        void RemoveRange(IEnumerable<int> ids);
    }
}
