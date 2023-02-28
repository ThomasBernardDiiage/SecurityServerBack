using System;
using API.BusinessLogic.Dto;


namespace API.BusinessLogic.Interfaces;

public interface IConnectionInformationService
{
    Task<IEnumerable<GetConnectionInformationResponseDto>> GetAllConnectionInformations();
    Task<SuccessAndFailedDto> GetSuccessAndFailedRequest();

}

