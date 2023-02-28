using System;
using API.BusinessLogic.Dto;
using API.BusinessLogic.Interfaces;
using API.DataAccess.Interfaces.Repositories;
using API.Interface.UnitOfWork;

namespace API.BusinessLogic;

public class ConnectionInformationService : IConnectionInformationService
{
    private readonly IUnitOfWork _unitOfWork;
    public ConnectionInformationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetConnectionInformationResponseDto>> GetAllConnectionInformations()
    {

        var connectionInformations = await _unitOfWork.GetRepository<IConnectionInformationRepository>().GetAllConnectionInformation();

        var dto = connectionInformations.Select(x =>
        new GetConnectionInformationResponseDto()
        {
            Id = x.Id,
            Date = x.Date,
            HttpResult = x.HttpResult,
            Ip = x.Ip,
            UserId = x.UserId
        });

        return dto;
    }

    public async Task<SuccessAndFailedDto> GetSuccessAndFailedRequest()
    {
        var allConnectionInformation = await _unitOfWork.GetRepository<IConnectionInformationRepository>().GetAllConnectionInformation();

        var numberSuccess = allConnectionInformation.Where(x => x.HttpResult >= 200 && x.HttpResult < 300).Count();
        var numberFailed = allConnectionInformation.Count() - numberSuccess;

        //var w = allConnectionInformation.GroupBy(x => x.UserId).Select(x => x.Key, x.Count().Count();

        return new SuccessAndFailedDto() { NumberOfSuccess = numberSuccess, NumberOfFailed = numberFailed};
    }
}

