using System;
using API.BusinessLogic.Interfaces;
using API.Models.Response.Applications;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using API.Middlewares;

namespace API.Applications;

public class DeleteApplication
{
    private readonly ILogger _logger;
    private readonly IApplicationService _applicationService;

    public DeleteApplication(ILoggerFactory loggerFactory, IApplicationService applicationService)
    {
        _logger = loggerFactory.CreateLogger<API>();
        _applicationService = applicationService;
    }

    [Authorize(new[] { "Admin" })]
    [Function("DeleteApplication")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "applications/{id}")] HttpRequestData req, ILogger log, int id)
    {
        var isApplicationDeleted = await _applicationService.DeleteApplicationAsync(id);

        return isApplicationDeleted ? req.CreateResponse(HttpStatusCode.OK) : req.CreateResponse(HttpStatusCode.NotFound);
    }
}
