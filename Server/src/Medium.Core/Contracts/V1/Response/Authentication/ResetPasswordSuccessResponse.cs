namespace Medium.Core.Contracts.V1.Response.Authentication
{
    public class ResetPasswordSuccessResponse
    {
        public bool Reseted { get; set; } = false;
        public string SuccessMessage { get; set; }
    }
}
