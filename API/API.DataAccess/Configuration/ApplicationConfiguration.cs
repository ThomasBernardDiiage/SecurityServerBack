using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DataAccess.Configuration
{
    internal class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder
                .ToTable("Application");

            builder
                .Property(e => e.Id)
                .HasColumnName("ApplicationId")
                .ValueGeneratedOnAdd();

            builder
                .Property(e => e.Name);

            builder
                .Property(e => e.Secret);
        }
    }
}
