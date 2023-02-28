using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using API.BusinessLogic.Interfaces;
using API.Models.Response.Users;
using System.Text.Json;
using API.Middlewares;

namespace API.Users;

public class ListUsers
{
    private readonly ILogger _logger;
    private readonly IUserService _userService;

    public ListUsers(ILoggerFactory loggerFactory, IUserService userService)
    {
        _logger = loggerFactory.CreateLogger<API>();
        _userService = userService;
    }

    [Authorize(new[] { "User" })]
    [Function("ListUsers")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequestData req)
    {
        var users = await _userService.GetUsersAsync();

        var userReponse = users.Select(u => new UserResponse
        {
            Id = u.Id,
            Email = u.Email,
            Firstname = u.Firstname,
            Lastname = u.Lastname,
            Picture = u.Picture
        });

        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var response = req.CreateResponse(HttpStatusCode.OK);

        response.WriteString(JsonSerializer.Serialize(userReponse, serializeOptions));

        return response;
    }
}

