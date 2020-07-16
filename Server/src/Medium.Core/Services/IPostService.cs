using Medium.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medium.Core.Services
{
    public interface IPostService
    {
        Task<Post> GetPostByIdAsync(Guid postId);
        Task<IEnumerable<Post>> GetPostsAsync(PaginationFilter paginationFilter = null);
        Task<bool> CreatePostAsync(Post post);
        Task<bool> UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(Post post);
        Task<bool> DeletePostAsync(Guid postId);
    }
}
