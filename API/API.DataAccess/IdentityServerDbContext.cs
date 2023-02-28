using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace API.EFCore
{
    public class IdentityServerDbContext : DbContext
    {
        private readonly string _dbConnection;
        public IdentityServerDbContext(string dbConnection)
        {
            _dbConnection = dbConnection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_dbConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}