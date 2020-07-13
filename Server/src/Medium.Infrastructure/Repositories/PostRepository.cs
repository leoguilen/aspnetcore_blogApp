using Medium.Core.Domain;
using Medium.Core.Repositories;
using Medium.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medium.Infrastructure.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(DataContext context) : base(context) { }

        public async Task CreatePostAsync(Post post)
        {
            await CreateAsync(post);
        }

        public async Task DeletePostAsync(Guid postId)
        {
            await DeleteAsync(postId);
        }

        public async Task DeletePostAsync(Post post)
        {
            await DeleteAsync(post);
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            var posts = await FindAllAsync();
            return await posts.ToListAsync();
        }

        public async Task<Post> GetByIdAsync(Guid postId)
        {
            var post = await FindByConditionAsync(post =>
                post.Id == postId);

            return await post.SingleOrDefaultAsync();
        }

        public async Task UpdatePostAsync(Post post)
        {
            await UpdateAsync(post);
        }
    }
}
