namespace Medium.Core.Contracts.V1.Request
{
    public class AuthorResetPasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}
