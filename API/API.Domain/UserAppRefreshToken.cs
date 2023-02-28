namespace API.Domain
{
    public class UserAppRefreshToken : BaseEntity
    {
        public int UserId { get; set; }
        public int ApplicationId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public long Expiration { get; set; }
    }
}
