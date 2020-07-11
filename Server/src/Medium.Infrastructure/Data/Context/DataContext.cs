using Medium.Core.Domain;
using Medium.Infrastructure.Data.Context.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Medium.Infrastructure.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
        }
    }
}
