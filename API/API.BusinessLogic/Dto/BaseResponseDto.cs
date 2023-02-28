using System;
namespace API.BusinessLogic.Dto;

public abstract class BaseResponseDto
{
    public string Message { get; set; }
    public bool Success { get; set; }

    public BaseResponseDto(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}
