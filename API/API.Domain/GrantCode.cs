using System;
namespace API.Domain;

public class GrantCode : BaseEntity
{
	public GrantCode()
	{
	}

	public string Value { get; set; }
	public int UserId { get; set; }
	public int ApplicationId { get; set; }
}

