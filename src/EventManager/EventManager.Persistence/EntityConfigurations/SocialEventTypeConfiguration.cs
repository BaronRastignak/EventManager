using EventManager.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManager.Persistence.EntityConfigurations;

internal class SocialEventTypeConfiguration : IEntityTypeConfiguration<SocialEvent>
{
    public void Configure(EntityTypeBuilder<SocialEvent> builder)
    {
        builder.ToTable("social_events", SocialEventsContext.DefaultSchema);

        builder.HasKey(e => e.Id);

        builder.Ignore(e => e.DomainEvents);

        builder.Property(e => e.Name).HasMaxLength(300).IsRequired();

        builder.Property(e => e.Date).IsRequired();
    }
}
