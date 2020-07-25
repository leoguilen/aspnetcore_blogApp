using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Medium.Infrastructure.Data.Context.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired(false);

            builder.HasData
            (
                new Tag
                {
                    Id = Guid.NewGuid(),
                    Name = "Tag 1"
                }
            );
        }
    }
}
