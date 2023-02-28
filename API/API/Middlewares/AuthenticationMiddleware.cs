using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;
using API.BusinessLogic.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace API.Middlewares;

public class AuthenticationMiddleware : IFunctionsWorkerMiddleware
{
    private readonly JwtSecurityTokenHandler _tokenValidator;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly ICertificateService _certificateService;


    // Verifiy token signature
    public AuthenticationMiddleware(ICertificateService certificateService)
	{
        _tokenValidator = new JwtSecurityTokenHandler();
        _certificateService = certificateService;
    }

    /// <summary>
    /// Verify if the bearer token is valid
    /// </summary>
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var method = context.GetTargetFunctionMethod(); // Get methods informations
        var isSecurizedRoute = method.CustomAttributes?.Any(x => x.AttributeType == typeof(AuthorizeAttribute)) ?? false; // check if method have [Authorize] attribute
        if (isSecurizedRoute) // If route is securized
        {
            if (!_certificateService.TryGetTokenFromHeaders(context, out var token)) // Get token from header
            {
                context.SetHttpResponseStatusCode(HttpStatusCode.Unauthorized);
                return;
            }


            if (!_tokenValidator.CanReadToken(token)) // Check lisibilty
            {
                context.SetHttpResponseStatusCode(HttpStatusCode.Unauthorized);
                return;
            }


            if (!_certificateService.VerifyCertificate(token)) // Verify if signed
            {
                context.SetHttpResponseStatusCode(HttpStatusCode.Forbidden);
                return;
            }
        }

        await next(context);
    }
}

