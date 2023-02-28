using System;
using API.BusinessLogic.Interfaces;
using API.Models.Response.Users;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using API.Middlewares;

namespace API.Users;

public class DeleteUser
{
    private readonly ILogger _logger;
    private readonly IUserService _userService;

    public DeleteUser(ILoggerFactory loggerFactory, IUserService userService)
    {
        _logger = loggerFactory.CreateLogger<API>();
        _userService = userService;
    }

    [Authorize(new[] { "User" })]
    [Function("DeleteUser")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "users/{id}")] HttpRequestData req, ILogger log, int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            return req.CreateResponse(HttpStatusCode.OK);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return req.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
