using Domain.Entities.UserAggregate;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("T_User");
        //To do
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PhoneNumber).HasConversion(x=>x.Value,x=> PhoneNumber.Create(x).Value).IsRequired().HasMaxLength(20);
        builder.Property(x => x.Email).HasConversion(x => x.Value, v => Email.Create(v).Value).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PasswordHash).HasMaxLength(100);
        builder.OwnsOne(x => x.Name, builder =>
        {
            builder.Property(x => x.Value)
                .HasColumnName("Name")
                .HasMaxLength(20)
                .IsUnicode(false);
        });
    }
}