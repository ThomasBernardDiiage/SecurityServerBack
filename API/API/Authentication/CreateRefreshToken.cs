using System.Net;
using System.Text.Json;
using API.BusinessLogic.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace API.Authentication;

public class CreateRefreshToken
{
    private readonly IAuthenticationService _authenticationService;
    public CreateRefreshToken(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [Function("CreateRefreshToken")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "refresh")] HttpRequestData req)
    {
        try
        {
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);

            var token = await _authenticationService.CreateAccessTokenByRefreshToken(query["refresh"]);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.WriteString(JsonSerializer.Serialize(token, serializeOptions));

            return response;
        }
        catch (Exception ex)
        {
            return req.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}

