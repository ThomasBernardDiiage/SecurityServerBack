using System;
using System.Net;
using System.Text.Json;
using API.BusinessLogic.Dto;
using API.BusinessLogic.Interfaces;
using API.Models.Request.Authentication;
using API.Models.Response.Authentication;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API.Authentication;


public class CreateCodeGrant
{
    private readonly IAuthenticationService _authenticationService;
    public CreateCodeGrant(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [Function("CreateAccess")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "grant")] HttpRequestData req)
    {
        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var createAccessTokenRequest = JsonConvert.DeserializeObject<CreateAccessTokenRequest>(requestBody);

            var dto = new GrantCodeDto()
            {
                ApplicationSecret = createAccessTokenRequest.Secret,
                Email = createAccessTokenRequest.Email,
                Password = createAccessTokenRequest.Password
            };

            var grant = await _authenticationService.CreateGrantCodeAsync(dto);

            var result = new CreateGrantCodeResponse
            {
                GrantCode = grant.Grant,
                Uri = grant.Uri
            };

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteString(System.Text.Json.JsonSerializer.Serialize(result, serializeOptions));
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            return response;
        }
        catch (Exception ex)
        {
            return req.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}