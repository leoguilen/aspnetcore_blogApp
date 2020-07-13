using Medium.Core.Domain;
using Medium.Core.Services;
using Medium.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medium.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            post.Id = Guid.NewGuid();

            await _unitOfWork.Posts.CreateAsync(post);
            var created = await _unitOfWork.Commit();

            return created > 0;
        }

        public async Task<bool> DeletePostAsync(Post post)
        {
            var deletePost = await GetPostByIdAsync(post.Id);

            if (deletePost == null)
                return false;

            await _unitOfWork.Posts.DeleteAsync(post);
            var deleted = await _unitOfWork.Commit();

            return deleted > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var deletePost = await GetPostByIdAsync(postId);

            if (deletePost == null)
                return false;

            await _unitOfWork.Posts.DeleteAsync(postId);
            var deleted = await _unitOfWork.Commit();

            return deleted > 0;
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _unitOfWork.Posts.GetByIdAsync(postId);
        }

        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            return await _unitOfWork.Posts.GetAllAsync();
        }

        public async Task<bool> UpdatePostAsync(Post post)
        {
            var updatePost = await GetPostByIdAsync(post.Id);

            if (updatePost == null)
                return false;

            await _unitOfWork.Posts.UpdateAsync(post);
            var updated = await _unitOfWork.Commit();

            return updated > 0;
        }
    }
}
