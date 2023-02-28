using System;
using API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.DataAccess.Configuration;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public UserRoleConfiguration()
    {
    }

    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("UserRole");

        builder.Property(e => e.Id)
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.UserId);
        builder.Property(e => e.RoleId);
    }
}
