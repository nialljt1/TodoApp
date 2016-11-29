using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Infrastructure
{
    public class BaseDbContext : DbContext
    {
        private const int ConstraintConflictErrorNumber = 547;

        public BaseDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public virtual bool DatabaseExists()
        {
            return DatabaseExists();
        }

        public virtual Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return ExecuteSqlCommandAsync(sql, parameters);     
        }

        public virtual TEntity SetAdded<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Added;
            return entity;
        }

        public virtual TEntity SetDeleted<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Deleted;
            return entity;
        }

        public virtual TEntity SetModified<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
            return entity;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Properties<DateTime>()
                .Configure(o => o.HasColumnType("datetime2").HasPrecision(0));

            modelBuilder
                .Properties<DateTime>()
                .Where(o => o.Name.EndsWith("On"))
                .Configure(o => o.HasColumnType("date"));

            modelBuilder
                .Properties<DateTimeOffset>()
                .Configure(o => o.HasPrecision(0));

            modelBuilder
                .Properties<TimeSpan>()
                .Configure(o => o.HasPrecision(0));

            modelBuilder.Properties<decimal>()
                .Configure(o => o.HasPrecision(22, 4));
        }
    }
}
