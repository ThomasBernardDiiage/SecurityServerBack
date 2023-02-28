using System;
using API.Domain;

namespace API.DataAccess.Interfaces.Repositories;

public interface IRoleRepository : IRepository
{
    Task CreateRoleAsync(Role role);
    Task<IEnumerable<Role>> GetRolesAsync(int userId);
    Task AffectRoleAsync(int userId, int roleId = 1);
}

