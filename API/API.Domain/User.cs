namespace API.Domain;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Picture { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}