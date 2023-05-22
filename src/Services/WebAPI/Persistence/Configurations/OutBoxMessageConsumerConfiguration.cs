using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Outbox;

namespace Persistence.Configurations;

public class OutBoxMessageConsumerConfiguration : IEntityTypeConfiguration<OutBoxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutBoxMessageConsumer> builder)
    {
        builder.ToTable("T_OutBoxMessageConsumer");
        builder.HasKey(x => x.Id);
    }
}