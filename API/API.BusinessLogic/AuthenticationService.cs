using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using API.BusinessLogic.Dto;
using API.BusinessLogic.Interfaces;
using API.BusinessLogic.Security;
using API.DataAccess.Interfaces.Repositories;
using API.Domain;
using API.Interface.UnitOfWork;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using AccessToken = API.BusinessLogic.Security.AccessToken;

namespace API.BusinessLogic;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICertificateService _certificateService;

    private readonly TokenOptions _tokenOptions;
    private readonly SigningConfigurations _signingConfigurations;

    public AuthenticationService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, ICertificateService certificateService, TokenOptions tokenOptions, SigningConfigurations signingConfigurations)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenOptions = tokenOptions;
        _signingConfigurations = signingConfigurations;
        _certificateService = certificateService;
    }

    public async Task<TokenResponseDto> CreateAccessTokenAsync(string grantCode)
    {
        var grantCodeRepo = await _unitOfWork.GetRepository<IGrantCodeRepository>().GetGrantCodeAsync(grantCode);
        var userRolesRepo = await _unitOfWork.GetRepository<IRoleRepository>().GetRolesAsync(grantCodeRepo.UserId);

        UserApplicationClaim[] userApplicationClaim = (await _unitOfWork.GetRepository<IUserApplicationClaimRepository>()
               .GetAllApplicationClaimsAsync(grantCodeRepo.UserId, grantCodeRepo.ApplicationId))
               .ToArray();

        UserApplicationClaimDto[] userApplicationClaimDtos = userApplicationClaim.Select(e => new UserApplicationClaimDto
        {
            Type = e.ApplicationClaim.ClaimType,
            Value = e.Value
        }).ToArray();

        var refreshToken = BuildRefreshToken();
        var accessToken = BuildAccessToken(grantCodeRepo.UserId, userApplicationClaimDtos, refreshToken, userRolesRepo);

        await _unitOfWork.GetRepository<IUserRefreshTokenRepository>().AddUserRefreshTokenAsync(grantCodeRepo.UserId, grantCodeRepo.ApplicationId, refreshToken.Token, refreshToken.Expiration);

        await _unitOfWork.GetRepository<IGrantCodeRepository>().DeleteGrantCodeAsync(grantCodeRepo.Id);

        await _unitOfWork.SaveChangesAsync();

        return new TokenResponseDto(true, string.Empty, accessToken);
    }

    public async Task<GrantResponseDto> CreateGrantCodeAsync(GrantCodeDto grantCodeDto)
    {

        var application = await _unitOfWork.GetRepository<IApplicationRepository>().GetApplicationAsync(grantCodeDto.ApplicationSecret);
        var user = await _unitOfWork.GetRepository<IUserRepository>().GetUserByEmailAsync(grantCodeDto.Email);

        if (user is not null && application is not null && _passwordHasher.PasswordMatches(grantCodeDto.Password, user.Password))
        {
            var grantCode = new GrantCode
            {
                ApplicationId = application.Id,
                UserId = user.Id,
                Value = Guid.NewGuid().ToString()
            };

            await _unitOfWork.GetRepository<IGrantCodeRepository>().CreateGrantCodeAsync(grantCode);
            await _unitOfWork.SaveChangesAsync();

            var result = new GrantResponseDto
            {
                Grant = grantCode.Value,
                Uri = application.Uri
            };

            return result;
        }
        else
        {
            throw new Exception();
        }
    }

    private AccessToken BuildAccessToken(int userId, IEnumerable<UserApplicationClaimDto> userApplicationClaimsDto, RefreshToken refreshToken, IEnumerable<Role> roles)
    {
        //var certificate = _certificateService.GetCertificate();
        X509Certificate2 c;

        var clientSecretCredential = new ClientSecretCredential("14bc5219-40ca-4d62-a8e4-7c97c1236349", "83e27da8-f392-4116-8c45-cd2066ad394b", "t1H8Q~P7DgIV26jcgL_kStLjSc6rTFyO-19n3aT0");

        var vaultUrl = "https://keyvaultd2p2g4preprod.vault.azure.net/";

        var client = new CertificateClient(new Uri(vaultUrl), clientSecretCredential);

        var secret = new SecretClient(new Uri(vaultUrl), clientSecretCredential);

        KeyVaultCertificateWithPolicy certificate = client.GetCertificate("2024di2p2g4");

        if (certificate.Policy?.Exportable != true)
        {
            c = new X509Certificate2(certificate.Cer);
            RSA rsa = c.GetRSAPrivateKey();
            RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(rsa);

            var accessTokenExpiration = DateTime.UtcNow.AddSeconds(_tokenOptions.AccessTokenExpiration);

            var securityToken = new JwtSecurityToken
            (
                    issuer: _tokenOptions.Issuer,
                    audience: _tokenOptions.Audience,
                    claims: GetClaims(userId, userApplicationClaimsDto, roles),
                    expires: accessTokenExpiration,
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256)
            );

            var handler = new JwtSecurityTokenHandler();
            var accessToken = handler.WriteToken(securityToken);

            return new AccessToken(accessToken, accessTokenExpiration.Ticks, refreshToken);
        }
        string[] segments = certificate.SecretId.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length != 3)
            throw new Exception("Le certificat n'est pas complet");

        string secretName = segments[1];
        string secretVersion = segments[2];

        KeyVaultSecret keyVaultSecret = secret.GetSecret(secretName, secretVersion);

        if ("application/x-pkcs12".Equals(keyVaultSecret.Properties.ContentType, StringComparison.InvariantCultureIgnoreCase))
        {
            byte[] pfx = Convert.FromBase64String(keyVaultSecret.Value);
            c = new X509Certificate2(pfx);
            RSA rsa = c.GetRSAPrivateKey();

            RsaSecurityKey rsaSecurityKey = new RsaSecurityKey(rsa);

            var accessTokenExpiration = DateTime.UtcNow.AddSeconds(_tokenOptions.AccessTokenExpiration);

            var securityToken = new JwtSecurityToken
            (
                    issuer: _tokenOptions.Issuer,
                    audience: _tokenOptions.Audience,
                    claims: GetClaims(userId, userApplicationClaimsDto, roles),
                    expires: accessTokenExpiration,
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256)
            );

            var handler = new JwtSecurityTokenHandler();
            var accessToken = handler.WriteToken(securityToken);

            return new AccessToken(accessToken, accessTokenExpiration.Ticks, refreshToken);
        }

        throw new NotSupportedException("Le certificat n'est pas au format application/x-pkcs12");

    }

    private RefreshToken BuildRefreshToken()
    {
        var refreshToken = new RefreshToken
        (
                token: _passwordHasher.HashPassword(Guid.NewGuid().ToString()),
                expiration: DateTime.UtcNow.AddSeconds(_tokenOptions.RefreshTokenExpiration).Ticks
        );

        return refreshToken;
    }

    private async Task<RefreshTokenDto> GetUserAppRefreshTokenAsync(string token)
    {
        var refreshToken = await _unitOfWork.GetRepository<IUserRefreshTokenRepository>().GetUserRefreshTokenAsync(token);

        await _unitOfWork.GetRepository<IUserRefreshTokenRepository>().DeleteUserRefreshTokenAsync(refreshToken.Id);

        return new RefreshTokenDto
        {
            UserId = refreshToken.UserId,
            ApplicationId = refreshToken.ApplicationId,
            RefreshToken = refreshToken.RefreshToken,
            Expiration = refreshToken.Expiration
        };
    }

    public async Task<TokenResponseDto> CreateAccessTokenByRefreshToken(string refreshToken)
    {
        var refreshTokenBdd = await GetUserAppRefreshTokenAsync(refreshToken);

        if(refreshToken is null) 
            return null;
        
        var userRolesRepo = await _unitOfWork.GetRepository<IRoleRepository>().GetRolesAsync(refreshTokenBdd.UserId);

        UserApplicationClaim[] userApplicationClaim = (await _unitOfWork.GetRepository<IUserApplicationClaimRepository>()
       .GetAllApplicationClaimsAsync(refreshTokenBdd.UserId, refreshTokenBdd.ApplicationId))
       .ToArray();

        UserApplicationClaimDto[] userApplicationClaimDtos = userApplicationClaim.Select(e => new UserApplicationClaimDto
        {
            Type = e.ApplicationClaim.ClaimType,
            Value = e.Value
        }).ToArray();

        var newRefreshToken = BuildRefreshToken();
        var accessToken = BuildAccessToken(refreshTokenBdd.UserId, userApplicationClaimDtos, newRefreshToken, userRolesRepo);

        return new TokenResponseDto(true, string.Empty, accessToken);
    } 

    private IEnumerable<Claim> GetClaims(int userId, IEnumerable<UserApplicationClaimDto> userApplicationClaims, IEnumerable<Role> roles)
    {
        var userAppClaims = userApplicationClaims.Select(c => new Claim(c.Type, c.Value)).ToList();
        var userIdClaim = new Claim("id", userId.ToString());

        var roleClaims = roles.Select(role => new Claim("roles", role?.Name)).ToList();
        var claimsIdentity = new ClaimsIdentity(roleClaims);

        userAppClaims.Add(userIdClaim);
        userAppClaims.AddRange(claimsIdentity.Claims);

        return userAppClaims.ToArray();
    }
}