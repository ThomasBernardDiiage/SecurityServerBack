using System;
namespace API.Models.Response.Authentication;

public class CreateGrantCodeResponse
{
    public CreateGrantCodeResponse()
    {
    }

    public string GrantCode { get; set; }
    public string Uri { get; set; }
}

