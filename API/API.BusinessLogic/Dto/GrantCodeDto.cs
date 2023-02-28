using System;
namespace API.BusinessLogic.Dto;

public class GrantCodeDto
{
	public string Email { get; set; }
	public string Password { get; set; }
	public string ApplicationSecret { get; set; }
}

