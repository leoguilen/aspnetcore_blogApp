using Medium.Core.Domain;
using Medium.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Medium.Infrastructure.Data.Context.Configuration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Bio).IsRequired(false);
            builder.Property(x => x.Avatar).IsRequired(false);

            var salt = SecurePasswordHasher.CreateSalt(8);

            builder.HasData
            (
                new Author
                {
                    Id = Guid.Parse("4f29ee19-ea4b-421d-a796-c2ee446becd2"),
                    FirstName = "Administrador",
                    LastName = "Master",
                    Username = "admin.master",
                    Password = SecurePasswordHasher.GenerateHash("Admin2020#", salt),
                    Salt = salt,
                    Email = "admin.master@email.com",
                    ConfirmedEmail = true,
                    Deactivated = false,
                    Deleted = false,
                }
            );
        }
    }
}
