using API.BusinessLogic;
using API.DataAccess.Interfaces.Repositories;
using API.Domain;
using API.Interface.UnitOfWork;
using Moq;
using System.Collections.Generic;

namespace API.UnitTests
{
    public class GetAllApplicationServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWork = new Mock<IUnitOfWork>();
        private readonly ApplicationService _applicationService;

        public GetAllApplicationServiceTest()
        {
            _applicationService = new ApplicationService(_unitOfWork.Object);
        }

        [Fact]
        public async Task Test1()
        {
            ApplicationClaim nameClaim = new ApplicationClaim { ClaimType = "string", ClaimValue = "name" };
            ApplicationClaim ageClaim = new ApplicationClaim { ClaimType = "number", ClaimValue = "age" };
            var claims = new ApplicationClaim[] { nameClaim, ageClaim };

            var app1 = new Application { ApplicationsClaims = claims, Id = 1, Name = "appName1", Secret = "1''éésdsddS", Uri = "" };
            var app2 = new Application { ApplicationsClaims = claims, Id = 2, Name = "appName2", Secret = "13242dsdsdds", Uri = "" };
            IEnumerable<Application> applications = new Application[] {app1, app2};

            _unitOfWork.Setup(x => x.GetRepository<IApplicationRepository>().GetAllApplicationsAsync())
                .Returns(Task.FromResult(applications));

            var result = await _applicationService.GetAllApplicationsAsync();

            Assert.Equal(applications.Count(), result.Count());
        }
    }
}
