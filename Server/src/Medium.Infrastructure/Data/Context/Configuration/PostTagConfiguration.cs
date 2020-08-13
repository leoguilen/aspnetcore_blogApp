using Medium.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Medium.Infrastructure.Data.Context.Configuration
{
    public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasKey(x => new { x.PostId, x.TagId });

            builder.HasData
            (
                new PostTag
                {
                    Id = Guid.NewGuid(),
                    PostId = Guid.Parse("040b026e-df88-4753-b96a-1fbc18498c9d"),
                    TagId = Guid.Parse("02ce73a0-768d-4ef3-8347-1fcf9522ee55")
                }
            );
        }
    }
}
