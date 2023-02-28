using System;
namespace API.Domain;

public class UserRole : BaseEntity
{
	public int UserId { get; set; }
	public int RoleId { get; set; }
}

