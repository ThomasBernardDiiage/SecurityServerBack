using API.EFCore;
using Microsoft.EntityFrameworkCore.Design;

namespace API.DataAccess;

internal class IdentityServerDbContextFactory : IDesignTimeDbContextFactory<IdentityServerDbContext>
{
    public IdentityServerDbContext CreateDbContext(string[] args)
    {
        const string connection = "Server=tcp:localhost;Initial Catalog=IdentityServer;Persist Security Info=False;User ID=sa;Password=Tkm@akpRYh4m?qo4;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";

        return new IdentityServerDbContext(connection);
    }
}
