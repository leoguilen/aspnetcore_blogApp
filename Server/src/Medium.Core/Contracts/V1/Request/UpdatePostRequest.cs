namespace Medium.Core.Contracts.V1.Request
{
    public class UpdatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Attachments { get; set; }
    }
}
