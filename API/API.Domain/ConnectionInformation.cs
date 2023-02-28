using System;
namespace API.Domain;

public class ConnectionInformation : BaseEntity
{
    public string Ip { get; set; }
    public DateTime Date { get; set; }
    public int HttpResult { get; set; }
    public int UserId { get; set; }
}

