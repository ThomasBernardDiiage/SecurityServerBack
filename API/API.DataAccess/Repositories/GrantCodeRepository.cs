using System;
using System.Net.Sockets;
using API.DataAccess.Interfaces.Repositories;
using API.Domain;
using API.EFCore;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace API.DataAccess.Repositories;

public class GrantCodeRepository : IGrantCodeRepository
{
    private readonly IdentityServerDbContext _context;
    public GrantCodeRepository(IdentityServerDbContext identityServerDbContext)
    {
        _context = identityServerDbContext;
    }

    public async Task CreateGrantCodeAsync(GrantCode grantCode)
    {
        await _context.Set<GrantCode>().AddAsync(grantCode);
    }

    public async Task<GrantCode> GetGrantCodeAsync(string value)
    {
        return await _context.Set<GrantCode>().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Value == value);
    }

    public async Task DeleteGrantCodeAsync(int id)
    {
        _context.Set<GrantCode>().Remove(new GrantCode { Id = id });
    }
}

