using System;
using API.BusinessLogic.Interfaces;
using API.Middlewares;
using API.Models.Response.Users;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using API.BusinessLogic.Dto;
using API.Models.Request.Users;
using Newtonsoft.Json;

namespace API.Users;

public class ModifyUser
{
    public IUserService _userService { get; set; }
    public ICertificateService _certificateService { get; set; }

    public ModifyUser(ILoggerFactory loggerFactory, IUserService userService, ICertificateService certificateService)
    {
        _userService = userService;
        _certificateService = certificateService;
    }

    [Authorize(new[] { "User" })]
    [Function("ModifyUser")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "users")] HttpRequestData req)
    {
        string requestBody = await new StreamReader(req.Body)
            .ReadToEndAsync();

        var request = JsonConvert.DeserializeObject<ModifyUserRequest>(requestBody);

        var userDto = new ModifyUserDto
        {
            Id = request.Id,
            Email = request.Email,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Picture = request.Picture
        };

        var createdUser = await _userService.ModifyUserAsync(userDto);
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

