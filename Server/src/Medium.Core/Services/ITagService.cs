using Medium.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medium.Core.Services
{
    public interface ITagService
    {
        Task<Tag> GetTagByIdAsync(Guid tagId);
        Task<IEnumerable<Tag>> GetTagsAsync(PaginationFilter paginationFilter = null);
        Task<bool> CreateTagAsync(Tag tag);
        Task<bool> UpdateTagAsync(Tag tag);
        Task<bool> DeleteTagAsync(Tag tag);
        Task<bool> DeleteTagAsync(Guid tagId);
    }
}
