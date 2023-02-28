using System;
using API.BusinessLogic.Interfaces;
using API.Middlewares;
using API.Models.Response.Applications;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using API.Models.Response.Authentication;

namespace API.Authentication;

public class GetConnectionInformations
{
    private readonly ILogger _logger;
    private readonly IConnectionInformationService _connectionInformationService;

    public GetConnectionInformations(ILoggerFactory loggerFactory, IConnectionInformationService connectionInformationService)
    {
        _logger = loggerFactory.CreateLogger<API>();
        _connectionInformationService = connectionInformationService;
    }

    //[Authorize(new[] { "User" })]
    [Function("GetConnectionInformations")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "connectionInformations")] HttpRequestData req)
    {
        var connectionInformations = await _connectionInformationService.GetAllConnectionInformations();

        var responseBody = connectionInformations.Select(x => new ConnectionInformationResponse { Ip = x.Ip, Id = x.Id, Date = x.Date, HttpResult = x.HttpResult, UserId = x.UserId});

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteString(JsonSerializer.Serialize(responseBody, serializeOptions));

        return response;
    }
}

public class GetSuccessAndFailedConnection
{
    private readonly ILogger _logger;
    private readonly IConnectionInformationService _connectionInformationService;

    public GetSuccessAndFailedConnection(ILoggerFactory loggerFactory, IConnectionInformationService connectionInformationService)
    {
        _logger = loggerFactory.CreateLogger<API>();
        _connectionInformationService = connectionInformationService;
    }

    //[Authorize(new[] { "User" })]
    [Function("GetSuccessAndFailedConnection")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getSuccessAndFailedConnection")] HttpRequestData req)
    {
        var connectionInformations = await _connectionInformationService.GetSuccessAndFailedRequest();

        var responseBody = new SucessAndFailedResponse() { NumberFailed = connectionInformations.NumberOfFailed, NumberSucess = connectionInformations.NumberOfSuccess };

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteString(JsonSerializer.Serialize(responseBody, serializeOptions));

        return response;
    }
}
