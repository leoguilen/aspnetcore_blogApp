using Medium.Core.Domain;
using System;

namespace Medium.Core.Common.Builder
{
    public class AuthorBuilder
    {
        private readonly Author _author;

        public AuthorBuilder()
        {
            _author = new Author();
        }

        public AuthorBuilder WithId(Guid authorId)
        {
            _author.Id = authorId;
            return this;
        }

        public AuthorBuilder WithFirstName(string firstName)
        {
            _author.FirstName = firstName;
            return this;
        }

        public AuthorBuilder WithLastName(string lastName)
        {
            _author.LastName = lastName;
            return this;
        }

        public AuthorBuilder WithUsername(string username)
        {
            _author.Username = username;
            return this;
        }

        public AuthorBuilder WithPassword(string password)
        {
            _author.Password = password;
            return this;
        }

        public AuthorBuilder WithSalt(string salt)
        {
            _author.Salt = salt;
            return this;
        }

        public AuthorBuilder WithEmail(string email)
        {
            _author.Email = email;
            return this;
        }

        public AuthorBuilder WithConfirmedEmail(bool confirmedEmail)
        {
            _author.ConfirmedEmail = confirmedEmail;
            return this;
        }

        public AuthorBuilder WithBio(string bio)
        {
            _author.Bio = bio;
            return this;
        }

        public AuthorBuilder WithAvatar(string avatar)
        {
            _author.Avatar = avatar;
            return this;
        }

        public AuthorBuilder WithDeactivated(bool deactivated)
        {
            _author.Deactivated = deactivated;
            return this;
        }

        public AuthorBuilder WithDeleted(bool deleted)
        {
            _author.Deleted = deleted;
            return this;
        }

        public Author Build()
        {
            return _author;
        }
    }
}
