using Medium.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Medium.Infrastructure.Data.Context.Configuration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).IsRequired(true);
            builder.Property(x => x.Content).IsRequired(false);

            builder.HasData
            (
                new Post
                {
                    Id = Guid.Parse("040b026e-df88-4753-b96a-1fbc18498c9d"),
                    Title = "Post Example",
                    Content = "Post added in migration",
                    Attachments = string.Empty,
                    AuthorId = Guid.Parse("4f29ee19-ea4b-421d-a796-c2ee446becd2")
                }
            );
        }
    }
}
