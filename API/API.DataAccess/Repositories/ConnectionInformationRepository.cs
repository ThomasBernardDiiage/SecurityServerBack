using System;
using API.DataAccess.Interfaces.Repositories;
using API.Domain;
using API.EFCore;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Repositories;

public class ConnectionInformationRepository : IConnectionInformationRepository
{
    private readonly IdentityServerDbContext _context;
    public ConnectionInformationRepository(IdentityServerDbContext identityServerDbContext)
    {
        _context = identityServerDbContext;
    }

    public async Task<IEnumerable<ConnectionInformation>> GetAllConnectionInformation()
    {
        return await _context.Set<ConnectionInformation>().ToListAsync();
    }

    public Task<(int Success, int Failed)> GetSuccessAndFailedRequest()
    {
        throw new NotImplementedException();
    }
}

