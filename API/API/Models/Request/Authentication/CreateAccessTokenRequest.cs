using System;
namespace API.Models.Request.Authentication;

public record CreateAccessTokenRequest(string Email, string Password, string Secret);
