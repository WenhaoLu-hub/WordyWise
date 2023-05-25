using Domain.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = Domain.Enums.Permission;

namespace Persistence.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("T_RolePermission");
        builder.HasKey(x => new { x.RoleId, x.PermissionId });
        builder.HasData(
            Create(Role.Registered, Permission.ReadUser),
            Create(Role.Registered, Permission.UpdateUser));
    }

    private static RolePermission Create(Role role, Permission permission)
    {
        return new RolePermission
        {
            PermissionId = (int)permission,
            RoleId = role.Id
        };
    }
}