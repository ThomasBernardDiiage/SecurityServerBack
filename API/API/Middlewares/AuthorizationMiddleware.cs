using System;
using System.Net;
using System.Reflection;
using API.BusinessLogic.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace API.Middlewares;

public class AuthorizationMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ICertificateService _certificateService;

	public AuthorizationMiddleware(ICertificateService certificateService)
	{
        _certificateService = certificateService;
    }

    /// <summary>
    /// Verify if the user have acces to the route
    /// </summary>
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var method = context.GetTargetFunctionMethod();
        var requiredRoles = ((IEnumerable<CustomAttributeTypedArgument>)method.CustomAttributes?.FirstOrDefault(x => x.AttributeType == typeof(AuthorizeAttribute))?.ConstructorArguments?.FirstOrDefault().Value)?.Select(x => x.Value);

        // If securized route
        if (requiredRoles?.Any() ?? false)  
        {
            if (!_certificateService.TryGetTokenFromHeaders(context, out var token))
            {
                context.SetHttpResponseStatusCode(HttpStatusCode.Unauthorized);
                return;
            }
            var userRoles = _certificateService.GetClaims(token).Where(c => c.Type == "roles").Select(c => c.Value);

            var isAllowed = requiredRoles.Any(x => userRoles.Contains(x));

            if (!isAllowed)
            {
                context.SetHttpResponseStatusCode(HttpStatusCode.Forbidden);
                return;
            }
            
        }

        await next(context);
    }
}
