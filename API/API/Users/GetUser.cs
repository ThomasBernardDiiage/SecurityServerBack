using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json;
using API.BusinessLogic;
using API.BusinessLogic.Interfaces;
using API.Middlewares;
using API.Models.Response.Users;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace API.Users;

public class GetUser
{
    private readonly ILogger _logger;
    private readonly IUserService _userService;
    private readonly ICertificateService _certificateService;

    public GetUser(ILoggerFactory loggerFactory, IUserService userService, ICertificateService certificateService)
    {
        _logger = loggerFactory.CreateLogger<API>();
        _userService = userService;
        _certificateService = certificateService;
    }

    [Authorize(new[] { "User" })]
    [Function("GetUser")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/me")] HttpRequestData req)
    {
        try
        {
            var userId = GetUserId(req);
            var user = await _userService.GetUserAsync(userId);

            var userReponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Picture = user.Picture
            };

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteString(JsonSerializer.Serialize(userReponse, serializeOptions));

            return response;
        }
        catch (Exception ex)
        {
            return req.CreateResponse(HttpStatusCode.NotFound);
        }
    }

    private int GetUserId(HttpRequestData req)
    {
        var token = req.Headers.FirstOrDefault(h => h.Key == "Authorization").Value.FirstOrDefault().Replace("Bearer ", "");

        var claims = _certificateService.GetClaims(token);

        var claimId = claims.First(claim => claim.Type == "id").Value;

        return Int32.Parse(claimId);
    }
}

