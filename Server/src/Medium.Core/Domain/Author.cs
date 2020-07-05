using Medium.Core.Common.Extension;
using System;

namespace Medium.Core.Domain
{
    public class Author : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public bool ConfirmedEmail { get; set; }
        public string Bio { get; set; }
        public string Avatar { get; set; }
        public bool Deactivated { get; set; }
        public bool Deleted { get; set; }

        public Author()
        {
            CreatedAt = DateTime.Now.DefaultFormat();
            UpdatedAt = DateTime.Now.DefaultFormat();
        }
    }
}
