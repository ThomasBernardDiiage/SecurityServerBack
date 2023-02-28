using System;
using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DataAccess.Configuration;

public class GrantCodeConfiguration : IEntityTypeConfiguration<GrantCode>
{
    public void Configure(EntityTypeBuilder<GrantCode> builder)
    {
        builder.ToTable("GrantCode");

        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserId);
        builder.Property(e => e.ApplicationId);
        builder.Property(e => e.Value);
    }
}

