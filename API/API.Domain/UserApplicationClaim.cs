namespace API.Domain;

public class UserApplicationClaim : BaseEntity
{
    public ApplicationClaim ApplicationClaim { get; set; } = new();
    public User User { get; set; } = new();
    public string Value { get; set; } = string.Empty;
}

