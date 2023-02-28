using API.BusinessLogic.Dto.ApplicationDtos;

namespace API.BusinessLogic.Interfaces;

public interface IApplicationService
{
    Task<IEnumerable<GetAllApplicationsDto>> GetAllApplicationsAsync();
    Task<GetAllApplicationsDto> CreateApplicationAsync(CreateApplicationDto createApplicationDto);
    Task<bool> DeleteApplicationAsync(int id);
}
