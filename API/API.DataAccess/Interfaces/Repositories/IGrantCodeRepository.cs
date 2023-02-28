using System;
using API.Domain;

namespace API.DataAccess.Interfaces.Repositories;

public interface IGrantCodeRepository : IRepository
{
    Task CreateGrantCodeAsync(GrantCode grantCode);
    Task<GrantCode> GetGrantCodeAsync(string value);
    Task DeleteGrantCodeAsync(int id);
}

