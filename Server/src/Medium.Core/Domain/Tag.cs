using Medium.Core.Common.Extension;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medium.Core.Domain
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public Guid AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public Author Author { get; set; }

        public Tag()
        {
            CreatedAt = DateTime.Now.DefaultFormat();
            UpdatedAt = DateTime.Now.DefaultFormat();
        }
    }
}