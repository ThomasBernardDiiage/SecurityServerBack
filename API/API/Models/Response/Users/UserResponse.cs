﻿using System;
namespace API.Models.Response.Users;

public class UserResponse
{
    public UserResponse()
    {
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Picture { get; set; }
}

