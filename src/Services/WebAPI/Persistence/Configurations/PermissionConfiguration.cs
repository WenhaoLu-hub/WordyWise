using Domain.Entities.RoleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("T_Permission");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Description).HasMaxLength(100);
        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Permissions)
            .UsingEntity<Dictionary<string, object>>(
                "T_RolePermission",
                a=> a.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                b=>b.HasOne<Permission>().WithMany().HasForeignKey("PermissionId")
            );


    }
}