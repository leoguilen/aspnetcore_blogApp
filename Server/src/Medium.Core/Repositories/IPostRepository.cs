using Medium.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medium.Core.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetAllAsync();
        Task<Post> GetByIdAsync(Guid postId);
        Task CreatePostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(Guid postId);
        Task DeletePostAsync(Post post);
    }
}
