namespace API.Domain
{
    public class ApplicationClaim : BaseEntity
    {
        public Application Application { get; set; } = new ();
        public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set; } = string.Empty;
    }
}
