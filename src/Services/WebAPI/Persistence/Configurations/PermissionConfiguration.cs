using Domain.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("T_Permission");
        builder.HasKey(x => x.Id);
        var permissions = Enum.GetValues<Domain.Enums.Permission>()
            .Select(x => new Permission
            {
                Id = (int)x,
                Name = x.ToString()
            });
        builder.HasData(permissions);
    }
}