using System;
using API.DataAccess.Interfaces.Repositories;
using API.Domain;
using API.EFCore;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly IdentityServerDbContext _context;
    public RoleRepository(IdentityServerDbContext identityServerDbContext)
    {
        _context = identityServerDbContext;
    }

    public async Task AffectRoleAsync(int userId, int roleId = 1)
    {
        await _context.Set<UserRole>().AddAsync(new UserRole() { RoleId = roleId, UserId = userId });
    }

    public async Task CreateRoleAsync(Role role)
    {
        await _context.Set<Role>().AddAsync(role);
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Role>> GetRolesAsync(int userId)
    {
        var userRoles = await _context.Set<UserRole>().Where(x => x.UserId == userId).ToListAsync();

        var roles = new List<Role>();

        foreach (var userRole in userRoles)
        {
            var role = await _context.Set<Role>().FirstOrDefaultAsync(x => x.Id == userRole.RoleId);
            roles.Add(role);
        }

        return roles;
    }
}

