using System;
namespace API.Models.Response.Authentication;

public class ConnectionInformationResponse
{
    public int Id { get; set; }
    public string Ip { get; set; }
    public DateTime Date { get; set; }
    public int HttpResult { get; set; }
    public int UserId { get; set; }
}

