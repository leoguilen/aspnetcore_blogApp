using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medium.Core.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<IEnumerable<Tag>> GetAllAsync();
        Task<Tag> GetByIdAsync(Guid tagId);
        Task CreateTagAsync(Tag tag);
        Task UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(Guid tagId);
        Task DeleteTagAsync(Tag tag);
    }
}
