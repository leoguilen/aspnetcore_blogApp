using Medium.Core.Repositories;
using Medium.Core.UnitOfWork;
using Medium.Infrastructure.Data.Context;
using Medium.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace Medium.Infrastructure.unitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dbContext;
        private IAuthorRepository _author;

        public UnitOfWork(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IAuthorRepository Authors => _author ?? 
            (_author = new AuthorRepository(_dbContext));

        public async Task<int> Commit()
        {
            int status = 0; 

            using var dbContextTransaction = 
                await _dbContext.Database
                    .BeginTransactionAsync();
            try
            {
                status = await _dbContext.SaveChangesAsync();
                await dbContextTransaction.CommitAsync();
            } 
            catch 
            {
                await dbContextTransaction.RollbackAsync();
            }

            return status;
        }
    }
}
