using API.DataAccess.Interfaces.Repositories;
using API.EFCore;
using API.Interface.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

namespace API.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityServerDbContext _context;
        private IDbContextTransaction? _transaction = null;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(IdentityServerDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync()
        {
            if (_transaction is not null)
            {
                await _transaction.CommitAsync();
            }
        }

        public async Task CreateTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public T GetRepository<T>() where T : IRepository
        {
            Type? type = null;

            Type? typeFromRepositories = _repositories
                .FirstOrDefault(e => e.Key.GetInterface(typeof(T).Name) is not null)
                .Key;

            if (typeFromRepositories is null)
            {
                type = Assembly.GetExecutingAssembly().GetExportedTypes()
                    .Where(e => e.Name.Contains("Repository"))
                    .FirstOrDefault(e => e.GetInterface(typeof(T).Name) is not null);

                if (type is null)
                {
                    throw new NullReferenceException("Can't load repository from assembly");
                }


                var instance = Activator.CreateInstance(type, _context);

                if (instance is null)
                {
                    throw new NullReferenceException("Can't load repository from assembly");
                }

                _repositories[type] = instance;

                return (T)_repositories[type];
            }

            return (T)_repositories[typeFromRepositories];
        }

        public async Task RollBackAsync()
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}