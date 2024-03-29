using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(16);
        builder.Property(x => x.Description).HasMaxLength(512);
        builder.Property(x => x.Color).HasMaxLength(7);
    }
}