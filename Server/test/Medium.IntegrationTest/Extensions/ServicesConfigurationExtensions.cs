using Medium.Core.Common.Builder;
using Medium.Core.Domain;
using Medium.Infrastructure.Data.Context;
using Medium.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Medium.IntegrationTest.Extensions
{
    public static class ServicesConfigurationExtensions
    {
        public static DataContext SeedTestData(this DataContext dataContext)
        {
            #region Authors

            var salt = SecurePasswordHasher.CreateSalt(8);
            var authors = new List<Author>
            {
                new AuthorBuilder()
                    .WithId(Guid.Parse("d4182477-0823-4908-be1d-af808e594306"))
                    .WithFirstName("João")
                    .WithEmail("joao@email.com")
                    .WithPassword(SecurePasswordHasher.GenerateHash("joao123", salt))
                    .WithSalt(salt)
                    .Build(),
                new AuthorBuilder()
                    .WithId(Guid.Parse("9ab3d110-71e1-418f-86eb-519146e7d702"))
                    .WithFirstName("Maria")
                    .WithEmail("maria@email.com")
                    .WithPassword(SecurePasswordHasher.GenerateHash("maria123", salt))
                    .WithSalt(salt)
                    .Build(),
            };

            dataContext.Authors.AddRange(authors);

            #endregion

            #region Posts

            var posts = new List<Post>
            {
                new Post
                {
                    Id = Guid.Parse("b65afc54-d766-4377-8c89-22662582174e"),
                    Title = "Post 1",
                    Content = "First post content",
                    Attachments = "post1img1.jpg,post1img2.jpg",
                    AuthorId = authors[0].Id
                },
                new Post
                {
                    Id = Guid.Parse("a06ba60c-c999-4de3-aa23-4f0c13bd71ad"),
                    Title = "Post 2",
                    Content = "Second post content",
                    Attachments = "post2img1.jpg,post2img2.jpg",
                    AuthorId = authors[1].Id
                }
            };

            dataContext.Posts.AddRange(posts);

            #endregion

            #region Tags
                
            var tags = new List<Tag> 
            {
                new Tag 
                {
                    Id = Guid.Parse("5d5e9a28-7c3e-4c2a-8098-b866eab33e61"),
                    Name = "Tag_1"
                }, 
                new Tag 
                {
                    Id = Guid.Parse("d94e6e00-96d0-4fc7-b621-c7746705b471"),
                    Name = "Tag_2"                    
                }, 
            };

            dataContext.Tags.AddRange(tags);

            #endregion

            dataContext.SaveChanges();

            foreach (var entity in dataContext.ChangeTracker.Entries())
                entity.State = EntityState.Detached;

            return dataContext;
        }
    }
}
