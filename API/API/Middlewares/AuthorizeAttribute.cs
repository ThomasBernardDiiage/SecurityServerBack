using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace API.Middlewares;

public class AuthorizeAttribute : Attribute
{
    public string[] Roles { get; set; }

    public AuthorizeAttribute(string[] roles)
    {
        Roles = roles;
    }
}

public enum Roles
{
    User,
    Admin
}