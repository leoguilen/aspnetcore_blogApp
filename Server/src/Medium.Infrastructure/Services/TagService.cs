using Medium.Core.Domain;
using Medium.Core.Services;
using Medium.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medium.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateTagAsync(Tag tag)
        {
            tag.Id = Guid.NewGuid();

            await _unitOfWork.Tags.CreateAsync(tag);
            var created = await _unitOfWork.Commit();

            return created > 0;
        }

        public async Task<bool> DeleteTagAsync(Tag tag)
        {
            var deleteTag = await GetTagByIdAsync(tag.Id);

            if (deleteTag == null)
                return false;

            await _unitOfWork.Tags.DeleteAsync(tag);
            var deleted = await _unitOfWork.Commit();

            return deleted > 0;
        }

        public async Task<bool> DeleteTagAsync(Guid tagId)
        {
            var deleteTag = await GetTagByIdAsync(tagId);

            if (deleteTag == null)
                return false;

            await _unitOfWork.Tags.DeleteAsync(tagId);
            var deleted = await _unitOfWork.Commit();

            return deleted > 0;
        }

        public async Task<Tag> GetTagByIdAsync(Guid tagId)
        {
            return await _unitOfWork.Tags.GetByIdAsync(tagId);
        }

        public async Task<IEnumerable<Tag>> GetTagsAsync(PaginationFilter paginationFilter = null)
        {
            if (paginationFilter == null)
            {
                return await _unitOfWork.Tags.GetAllAsync();
            }

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;

            var tags = _unitOfWork.Tags
                .GetAllAsync()
                .Result
                .Skip(skip)
                .Take(paginationFilter.PageSize);

            return await Task.FromResult(tags);
        }

        public async Task<bool> UpdateTagAsync(Tag tag)
        {
            var updateTag = await GetTagByIdAsync(tag.Id);

            if (updateTag == null)
                return false;

            await _unitOfWork.Tags.UpdateAsync(tag);
            var updated = await _unitOfWork.Commit();

            return updated > 0;
        }
    }
}
