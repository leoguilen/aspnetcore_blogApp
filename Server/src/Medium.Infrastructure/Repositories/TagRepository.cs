using Medium.Core.Repositories;
using Medium.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medium.Infrastructure.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(DataContext context) : base(context) { }

        public async Task CreateTagAsync(Tag tag)
        {
            await CreateAsync(tag);
        }

        public async Task DeleteTagAsync(Guid tagId)
        {
            await DeleteAsync(tagId);
        }

        public async Task DeleteTagAsync(Tag tag)
        {
            await DeleteAsync(tag);
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            var tags = await FindAllAsync();
            return await tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(Guid tagId)
        {
            var tag = await FindByConditionAsync(tag =>
                tag.Id == tagId);

            return await tag.SingleOrDefaultAsync();
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            await UpdateAsync(tag);
        }
    }
}
