namespace Medium.Core.Contracts.V1.Request.Author
{
    public class CreateAuthorRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string Avatar { get; set; }
    }
}
