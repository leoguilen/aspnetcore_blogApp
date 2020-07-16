namespace Medium.Core.Contracts.V1.Request.Post
{
    public class CreatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Attachments { get; set; }
    }
}
