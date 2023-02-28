using API.BusinessLogic.Dto;

namespace API.BusinessLogic.Interfaces;

public interface IAuthenticationService
{
    Task<TokenResponseDto> CreateAccessTokenAsync(string grantCode);
    Task<GrantResponseDto> CreateGrantCodeAsync(GrantCodeDto grantCodeDto);

    Task<TokenResponseDto> CreateAccessTokenByRefreshToken(string token);
}