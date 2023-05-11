using Domain.Entities.RoleAggregate;
using Domain.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("T_UserRole");

        builder.HasKey(x => new { x.UserId , x.RoleId });

        builder.HasOne<Role>()
            .WithMany(x =>x.UserRoles)
            .HasForeignKey(x => x.RoleId)
            .IsRequired();
    }
}