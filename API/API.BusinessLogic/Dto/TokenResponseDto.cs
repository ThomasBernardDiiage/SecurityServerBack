using System;
using API.BusinessLogic.Security;

namespace API.BusinessLogic.Dto; 

public class TokenResponseDto : BaseResponseDto
{
    public AccessToken? Token { get; set; }

    public TokenResponseDto(bool success, string message, AccessToken? accessToken) : base(success, message)
    {
        Token = accessToken;
    }
}

