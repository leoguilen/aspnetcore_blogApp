using Medium.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Medium.Infrastructure.Data.Context.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired(false);
            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasData
            (
                new Tag
                {
                    Id = Guid.Parse("02ce73a0-768d-4ef3-8347-1fcf9522ee55"),
                    Name = "Tag 1",
                    AuthorId = Guid.Parse("4f29ee19-ea4b-421d-a796-c2ee446becd2")
                }
            );
        }
    }
}
