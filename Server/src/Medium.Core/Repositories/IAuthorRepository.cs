using Medium.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medium.Core.Repositories
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author> GetByIdAsync(Guid authorId);
        Task CreateAuthorAsync(Author author);
        Task UpdateAuthorAsync(Author author);
        Task DeleteAuthorAsync(Guid authorId);
        Task DeleteAuthorAsync(Author author);
    }
}
