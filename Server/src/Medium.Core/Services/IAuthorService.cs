using Medium.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medium.Core.Services
{
    public interface IAuthorService
    {
        Task<Author> GetAuthorByIdAsync(Guid authorId);
        Task<IEnumerable<Author>> GetAuthorsAsync();
        Task<bool> CreateAuthorAsync(Author author);
        Task<bool> UpdateAuthorAsync(Author author);
        Task<bool> DeleteAuthorAsync(Author author);
        Task<bool> DeleteAuthorAsync(Guid authorId);
    }
}
