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
                    Id = Guid.NewGuid(),
                    Title = "Post Example",
                    Content = "Post added in migration",
                    Attachments = string.Empty
                }
            );
        }
    }
}
