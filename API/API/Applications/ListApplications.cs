using System;
using API.BusinessLogic.Dto.ApplicationDtos;
using System.Net;
using API.BusinessLogic.Interfaces;
using API.Models.Request.Applications;
using API.Models.Response.Applications;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using API.Middlewares;
using System.Text.Json;

namespace API.Applications;

public class ListApplications
{
    private readonly ILogger _logger;
    private readonly IApplicationService _applicationService;

    public ListApplications(ILoggerFactory loggerFactory, IApplicationService applicationService)
    {
        _logger = loggerFactory.CreateLogger<API>();
        _applicationService = applicationService;
    }

    [Authorize(new[] { "User" })]
    [Function("ListApplications")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "applications")] HttpRequestData req)
    {
        var applications = await _applicationService.GetAllApplicationsAsync();

        var responseBody = applications.Select(x => new ApplicationResponse
        {
            Id = x.Id,
            ApplicationName = x.ApplicationName,
            ApplicationSecret = x.ApplicationSecret
        });

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteString(JsonSerializer.Serialize(responseBody, serializeOptions));

        return response;
    }
}
