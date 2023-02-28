using System;
namespace API.Models.Request.Users;

public class CreateUserRequest
{
    public string Email { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Picture { get; set; }
    public string Password { get; set; }

}
