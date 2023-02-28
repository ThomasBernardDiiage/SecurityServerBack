using API.DataAccess.Interfaces.Repositories;

namespace API.Interface.UnitOfWork
{
    public interface IUnitOfWork
    {
        T GetRepository<T>() where T : IRepository;
        Task CreateTransactionAsync();
        Task CommitAsync();
        Task RollBackAsync();
        Task SaveChangesAsync();
    }
}
