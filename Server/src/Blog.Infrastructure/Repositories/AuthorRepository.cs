using Medium.Core.Domain;
using Medium.Core.Repositories;
using Medium.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medium.Infrastructure.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(DataContext context) : base(context) { }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            var authors = await FindAllAsync();

            return await authors.ToListAsync();
        }

        public async Task<Author> GetByIdAsync(Guid authorId)
        {
            var author = await FindByConditionAsync(author =>
                author.Id == authorId);

            return await author.SingleOrDefaultAsync();
        }

        public async Task CreateAuthorAsync(Author author)
        {
            await CreateAsync(author);
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            await UpdateAsync(author);
        }

        public async Task DeleteAuthorAsync(Guid authorId)
        {
            await DeleteAsync(authorId);
        }

        public async Task DeleteAuthorAsync(Author author)
        {
            await DeleteAsync(author);
        }
    }
}
