using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DataAccess.Configuration
{
    public class UserAppRefreshTokenConfiguration : IEntityTypeConfiguration<UserAppRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserAppRefreshToken> builder)
        {
            builder
                .ToTable(nameof(UserAppRefreshToken));

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId);

            builder
                .HasOne<Application>()
                .WithMany()
                .HasForeignKey(e => e.ApplicationId);

            builder
                .Property(e => e.RefreshToken);

            builder
                .Property(e => e.Expiration);
        }
    }
}
