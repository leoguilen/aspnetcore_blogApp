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
                    Attachments = "post1img1.jpg,post1img2.jpg"
                },
                new Post
                {
                    Id = Guid.Parse("a06ba60c-c999-4de3-aa23-4f0c13bd71ad"),
                    Title = "Post 2",
                    Content = "Second post content",
                    Attachments = "post2img1.jpg,post2img2.jpg"
                }
            };

            dataContext.Posts.AddRange(posts);

            #endregion

            dataContext.SaveChanges();

            foreach (var entity in dataContext.ChangeTracker.Entries())
                entity.State = EntityState.Detached;

            return dataContext;
        }
    }
}
