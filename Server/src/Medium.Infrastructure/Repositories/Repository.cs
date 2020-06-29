using Medium.Core.Repositories;
using Medium.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Medium.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext Context;

        protected Repository(DataContext context)
        {
            Context = context;
        }

        public async Task CreateAsync(T entity)
        {
            await Context.Set<T>()
                .AddAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            await Task.Run(() =>
                Context
                    .Set<T>()
                    .Remove(entity));
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await Context.Set<T>().FindAsync(id);
            await Task.Run(() =>
                Context
                    .Set<T>()
                    .Remove(entity));
        }

        public async Task<IQueryable<T>> FindAllAsync()
        {
            return await Task.Run(() =>
                Context
                    .Set<T>()
                    .AsNoTracking());
        }

        public async Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await Task.Run(() =>
                Context
                    .Set<T>()
                    .Where(expression)
                    .AsNoTracking());
        }

        public async Task UpdateAsync(T entity)
        {
            await Task.Run(() =>
                Context
                    .Set<T>()
                    .Update(entity));
        }
    }
}
