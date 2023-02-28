using System;
namespace API.BusinessLogic.Dto;

public class GetConnectionInformationResponseDto
{
	public GetConnectionInformationResponseDto()
	{
	}

    public int Id { get; set; }
    public string Ip { get; set; }
    public DateTime Date { get; set; }
    public int HttpResult { get; set; }
    public int UserId { get; set; }
}

