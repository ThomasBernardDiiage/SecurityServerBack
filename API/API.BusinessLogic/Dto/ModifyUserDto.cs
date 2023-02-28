using System;
namespace API.BusinessLogic.Dto;

public class ModifyUserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Picture { get; set; }
}

