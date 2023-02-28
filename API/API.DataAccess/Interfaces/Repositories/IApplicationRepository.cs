using API.Domain;

namespace API.DataAccess.Interfaces.Repositories
{
    public interface IApplicationRepository : IRepository
    {
        Task CreateApplicationAsync(Application application);
        Task<Application?> GetApplicationAsync(string secret);
        Task<Application?> GetApplicationAsync(int id);
        Task<IEnumerable<Application>> GetAllApplicationsAsync();
        Task DeleteApplicationAsync(Application application);
    }
}
