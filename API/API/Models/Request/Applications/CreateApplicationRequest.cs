using System;
namespace API.Models.Request.Applications;

public record CreateApplicationRequest(string ApplicationName, ApplicationClaimModel[] ApplicationClaims, string ApplicationUri);

public record ApplicationClaimModel(string Type, string Value);

