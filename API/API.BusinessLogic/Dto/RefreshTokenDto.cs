using System;
namespace API.BusinessLogic.Dto
{
	public class RefreshTokenDto
	{
		public RefreshTokenDto()
		{

		}

        public int UserId { get; set; }
        public int ApplicationId { get; set; }
        public string RefreshToken { get; set; }
		public long Expiration { get; set; }
	}
}