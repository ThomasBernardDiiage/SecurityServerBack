using System;
using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DataAccess.Configuration;

public class UserApplicationClaimConfiguration : IEntityTypeConfiguration<UserApplicationClaim>
{

    public void Configure(EntityTypeBuilder<UserApplicationClaim> builder)
    {
        builder.ToTable(nameof(UserApplicationClaim));

        builder.Property(e => e.Id).ValueGeneratedOnAdd();
    }
}

