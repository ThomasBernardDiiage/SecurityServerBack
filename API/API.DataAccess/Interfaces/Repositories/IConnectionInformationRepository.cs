using System;
using API.Domain;

namespace API.DataAccess.Interfaces.Repositories;


public interface IConnectionInformationRepository : IRepository
{
    Task<IEnumerable<ConnectionInformation>> GetAllConnectionInformation();

}

