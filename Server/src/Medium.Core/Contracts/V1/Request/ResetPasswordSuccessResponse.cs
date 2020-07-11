namespace Medium.Core.Contracts.V1.Request
{
    public class ResetPasswordSuccessResponse
    {
        public bool Reseted { get; set; } = false;
        public string SuccessMessage { get; set; }
    }
}
