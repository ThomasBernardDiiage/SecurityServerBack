using System;
using API.BusinessLogic.Dto;
using API.BusinessLogic.Interfaces;
using API.Models.Request.Users;
using API.Models.Response.Users;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using API.Models.Request.Applications;
using API.BusinessLogic.Dto.ApplicationDtos;
using API.BusinessLogic;
using API.Models.Response.Applications;
using API.Middlewares;
using System.Text.Json;

namespace API.Applications;

public class CreateApplication
{
    private readonly ILogger _logger;
    private readonly IApplicationService _applicationService;

    public CreateApplication(ILoggerFactory loggerFactory, IApplicationService applicationService)
    {
        _logger = loggerFactory.CreateLogger<API>();
        _applicationService = applicationService;
    }

    [Authorize(new[] { "User" })]
    [Function("CreateApplication")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "applications")] HttpRequestData req)
    {
        string requestBody = await new StreamReader(req.Body)
            .ReadToEndAsync();

        var request = JsonConvert.DeserializeObject<CreateApplicationRequest>(requestBody);

        var applicationDto = new CreateApplicationDto
        (
            request.ApplicationName,
            request.ApplicationClaims.Select(e => new ApplicationClaimDto(e.Type, e.Value)).ToArray(),
            request.ApplicationUri
        );

        var createdApplication = await _applicationService.CreateApplicationAsync(applicationDto);

        var responseBody = new ApplicationResponse
        {
            Id = createdApplication.Id,
            ApplicationName = createdApplication.ApplicationName,
            ApplicationSecret = createdApplication.ApplicationSecret
        };

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.WriteString(System.Text.Json.JsonSerializer.Serialize(responseBody, serializeOptions));

        return response;
    }
}
