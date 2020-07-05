using System;

namespace Medium.Core.Contracts.V1.Response
{
    public class AuthorResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool ConfirmedEmail { get; set; }
        public string Bio { get; set; }
        public string Avatar { get; set; }
        public bool Deactivated { get; set; }
        public bool Deleted { get; set; }
    }
}
