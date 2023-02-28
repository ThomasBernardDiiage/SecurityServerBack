using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DataAccess.Configuration
{
    internal class ApplicationClaimConfiguration : IEntityTypeConfiguration<ApplicationClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationClaim> builder)
        {
            builder
                .ToTable("ApplicationClaim");

            builder
               .Property(e => e.Id)
               .HasColumnName("ApplicationClaimsId")
               .ValueGeneratedOnAdd();

            builder
                .Property(e => e.ClaimType);

            builder
                .Property(e => e.ClaimValue);
        }
    }
}
