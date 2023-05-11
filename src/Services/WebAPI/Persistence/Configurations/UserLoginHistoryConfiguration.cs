using Domain.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserLoginHistoryConfiguration : IEntityTypeConfiguration<UserLoginHistory>
{
    public void Configure(EntityTypeBuilder<UserLoginHistory> builder)
    {
        builder.ToTable("T_UserLoginHistory");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LoginTime).IsRequired();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x =>x.UserId)
            .IsRequired();
    }
}