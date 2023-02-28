using API.BusinessLogic;
using API.BusinessLogic.Dto.ApplicationDtos;
using API.DataAccess.Interfaces.Repositories;
using API.Domain;
using API.Interface.UnitOfWork;
using Moq;

namespace API.UnitTests
{
    public class CreateApplicationServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
        private readonly ApplicationService _applicationService;

        public CreateApplicationServiceTest()
        {
           _applicationService = new ApplicationService(_unitOfWork.Object);
        }

        [Fact]
        public async Task Test1()
        {
            var applicationClaimDto1 = new ApplicationClaimDto("string", "nom");
            var applicationClaimDto2 = new ApplicationClaimDto("number", "age");

            var applicationClaimsDto = new ApplicationClaimDto[] { applicationClaimDto1, applicationClaimDto2 };

            CreateApplicationDto createApplicationDto = new CreateApplicationDto("appName", applicationClaimsDto, "");

            _unitOfWork.Setup(x => x.GetRepository<IApplicationRepository>()).Returns(new Mock<IApplicationRepository>().Object);

            await _applicationService.CreateApplicationAsync(createApplicationDto);
           
            _unitOfWork.Verify(e => e.GetRepository<IApplicationRepository>().CreateApplicationAsync(It.IsAny<Application>()), Times.Once);
            _unitOfWork.Verify(e => e.SaveChangesAsync(), Times.Once);
        }
    }
}