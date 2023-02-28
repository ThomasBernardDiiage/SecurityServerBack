using System;
using API.BusinessLogic;
using System.Net;
using API.BusinessLogic.Interfaces;
using API.Middlewares;
using API.Models.Response.Users;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using API.BusinessLogic.Dto;
using Newtonsoft.Json;
using API.Models.Request.Authentication;
using API.Models.Response.Authentication;
using Azure;
using System.Text.Json;

namespace API.Authentication;

public class CreateAccessToken
{
    private readonly IAuthenticationService _authenticationService;
    public CreateAccessToken(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [Function("CreateCodeGrant")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "accessToken")] HttpRequestData req)
    {
        try
        {
            //var code = req.Query["code"].ToString();
            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);

            var token = await _authenticationService.CreateAccessTokenAsync(query["code"]);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.WriteString(System.Text.Json.JsonSerializer.Serialize(token, serializeOptions));

            return response;
        }
        catch (Exception ex)
        {
            return req.CreateResponse(HttpStatusCode.NotFound);
        }

    }
}