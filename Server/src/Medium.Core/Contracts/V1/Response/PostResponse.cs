using System;

namespace Medium.Core.Contracts.V1.Response
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Attachments { get; set; }
    }
}
