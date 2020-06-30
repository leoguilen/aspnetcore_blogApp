using Medium.Core.Domain;
using Medium.Core.Services;
using Medium.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medium.Infrastructure.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateAuthorAsync(Author author)
        {
            var newAuthor = await _unitOfWork.Authors
                .FindByConditionAsync(a => a.Email == author.Email
                    || a.Username == author.Username)
                .Result.SingleOrDefaultAsync();

            if (newAuthor != null)
                return false;

            await _unitOfWork.Authors.CreateAsync(author);
            var created = await _unitOfWork.Commit();
            return created > 0;
        }

        public async Task<Author> GetAuthorByIdAsync(Guid authorId)
        {
            return await _unitOfWork.Authors.GetByIdAsync(authorId);
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await _unitOfWork.Authors.GetAllAsync();
        }

        public async Task<bool> UpdateAuthorAsync(Author author)
        {
            var updateAuthor = await GetAuthorByIdAsync(author.Id);

            if (updateAuthor == null)
                return false;

            await _unitOfWork.Authors.UpdateAsync(author);
            var updated = await _unitOfWork.Commit();
            return updated > 0;
        }

        public async Task<bool> DeleteAuthorAsync(Author author)
        {
            var deleteAuthor = await GetAuthorByIdAsync(author.Id);

            if (deleteAuthor == null)
                return false;

            await _unitOfWork.Authors.DeleteAsync(author);
            var deleted = await _unitOfWork.Commit();
            return deleted > 0;
        }

        public async Task<bool> DeleteAuthorAsync(Guid authorId)
        {
            var deleteAuthor = await GetAuthorByIdAsync(authorId);

            if (deleteAuthor == null)
                return false;

            await _unitOfWork.Authors.DeleteAsync(authorId);
            var deleted = await _unitOfWork.Commit();
            return deleted > 0;
        }
    }
}
