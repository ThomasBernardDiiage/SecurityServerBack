namespace API.BusinessLogic.Dto;

public record CreateAccessTokenDto(string email, string password, string secret);
