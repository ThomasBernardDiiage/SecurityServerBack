using System;

namespace API.BusinessLogic.Dto.ApplicationDtos
{
    public class GetAllApplicationsDto
    {
        public int Id { get; set; }

        public string ApplicationName { get; set; } = string.Empty;

        public string ApplicationSecret { get; set; } = string.Empty;
    }
}

