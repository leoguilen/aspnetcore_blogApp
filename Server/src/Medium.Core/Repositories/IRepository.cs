using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Medium.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> FindAllAsync();
        Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
