using Medium.Core.Common.Extension;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medium.Core.Domain
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Attachments { get; set; }
        public Guid AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public Author Author { get; set; }

        public Post()
        {
            CreatedAt = DateTime.Now.DefaultFormat();
            UpdatedAt = DateTime.Now.DefaultFormat();
        }
    }
}
