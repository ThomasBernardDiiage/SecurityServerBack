using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DataAccess.Configuration;

internal class ConnectionInformationConfiguration : IEntityTypeConfiguration<ConnectionInformation>
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

    public void Configure(EntityTypeBuilder<ConnectionInformation> builder)
    {
        builder.ToTable(nameof(ConnectionInformation));

        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.Ip);
        builder.Property(e => e.UserId);
        builder.Property(e => e.HttpResult);
        builder.Property(e => e.Date);
    }
}
