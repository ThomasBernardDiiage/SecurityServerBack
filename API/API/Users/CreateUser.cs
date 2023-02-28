using System;
using API.Models.Response.Users;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;
using API.BusinessLogic.Interfaces;
using Microsoft.Extensions.Logging;
using API.BusinessLogic.Dto;
using System.Text.Json.Serialization;
using API.Models.Request.Users;
using System.Net.Http.Json;
using Newtonsoft.Json;
using API.Middlewares;

namespace API.Users;

public class CreateUser
{
    private readonly ILogger _logger;
    private readonly IUserService _userService;

    public CreateUser(ILoggerFactory loggerFactory, IUserService userService)
    {
        _logger = loggerFactory.CreateLogger<API>();
        _userService = userService;
    }

    [Authorize(new[] { "User" })]
    [Function("CreateUser")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequestData req)
    {
        string requestBody = await new StreamReader(req.Body)
            .ReadToEndAsync();

        var request = JsonConvert.DeserializeObject<CreateUserRequest>(requestBody);

        var userDto = new CreateUserDto
        {
            Email = request.Email,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Password = request.Password,
            Picture = request.Picture
        };

        var createdUser = await _userService.CreateUserAsync(userDto);
        var responseBody = new UserResponse
        {
            Id = createdUser.Id,
            Email = createdUser.Email,
            Firstname = createdUser.Firstname,
            Lastname = createdUser.Lastname,
            Picture = createdUser.Picture,
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

