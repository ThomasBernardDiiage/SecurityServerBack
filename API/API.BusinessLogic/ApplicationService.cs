using API.BusinessLogic.Dto;
using API.BusinessLogic.Dto.ApplicationDtos;
using API.BusinessLogic.Interfaces;
using API.DataAccess.Interfaces.Repositories;
using API.Domain;
using API.Interface.UnitOfWork;
using ApplicationClaim = API.Domain.ApplicationClaim;

namespace API.BusinessLogic
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetAllApplicationsDto>> GetAllApplicationsAsync()
        {
            var applications = (await _unitOfWork.GetRepository<IApplicationRepository>()
                .GetAllApplicationsAsync())
                .ToArray();

            return applications.Select(application => new GetAllApplicationsDto
            {
                Id = application.Id,
                ApplicationName = application.Name,
                ApplicationSecret = application.Secret
            });
        }

        public async Task<GetAllApplicationsDto> CreateApplicationAsync(CreateApplicationDto createApplicationDto)
        {
            var claims = createApplicationDto.ApplicationClaims
                .Select(appClaim => new ApplicationClaim()
                {
                    ClaimType = appClaim.Type,
                    ClaimValue = appClaim.Value
                })
                .ToArray();

            var application = new Application
            {
                Name = createApplicationDto.ApplicationName,
                ApplicationsClaims = claims,
                Secret = Guid.NewGuid().ToString(),
                Uri = createApplicationDto.ApplicationUri
            };

            await _unitOfWork.GetRepository<IApplicationRepository>().CreateApplicationAsync(application);
            await _unitOfWork.SaveChangesAsync();

            return new GetAllApplicationsDto
            {
                Id = application.Id,
                ApplicationName = application.Name,
                ApplicationSecret = application.Secret
            };
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            var application = await _unitOfWork.GetRepository<IApplicationRepository>().GetApplicationAsync(id);

            if (application is not null)
            {
                await _unitOfWork.GetRepository<IApplicationRepository>().DeleteApplicationAsync(application);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }

            return false;
        }

    }
}