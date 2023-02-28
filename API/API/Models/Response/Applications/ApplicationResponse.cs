using System;
namespace API.Models.Response.Applications;

public class ApplicationResponse
{
	public ApplicationResponse()
	{
	}

    public int Id { get; set; }
    public string ApplicationName { get; set; } = string.Empty;
    public string ApplicationSecret { get; set; } = string.Empty;
}

