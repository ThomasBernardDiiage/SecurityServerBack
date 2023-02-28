namespace API.Domain
{
    public class Application : BaseEntity
    {
        public string Secret { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Uri { get; set; } = string.Empty;
        public IList<ApplicationClaim> ApplicationsClaims { get; set; } = new List<ApplicationClaim>();
    }
}
