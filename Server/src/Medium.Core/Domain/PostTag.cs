using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medium.Core.Domain
{
    public class PostTag
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid TagId { get; set; }

        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        [ForeignKey(nameof(TagId))]
        public Tag Tag { get; set; }
    }
}
