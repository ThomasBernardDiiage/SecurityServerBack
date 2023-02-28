namespace API.BusinessLogic.Dto.ApplicationDtos
{
    public record CreateApplicationDto(string ApplicationName,
        ApplicationClaimDto[] ApplicationClaims, string ApplicationUri);

    public record ApplicationClaimDto(string Type, string Value);
}
