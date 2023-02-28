using System;
namespace API.Models.Response.Authentication
{
	public class CreateRefreshTokenResponse
	{
		public CreateRefreshTokenResponse()
		{
		}

		public string RefreshToken { get; set; }
		public long Expiration { get; set; }
	}
}