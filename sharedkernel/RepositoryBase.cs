using System;
using Microsoft.EntityFrameworkCore;
using sharedkernel;
using sharedkernel.Interfaces;
using System.Linq.Expressions;

namespace sharedkernel
{
    public class RepositoryBase<TContext, TEntity> : IRepository<TEntity> where TEntity : class, IEntity where TContext : DbContext
    {
        protected readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbset;

        public RepositoryBase(TContext mainContext)
        {
            _dbContext = mainContext ?? throw new ArgumentNullException("Database Context can not be null.");
            _dbset = _dbContext.Set<TEntity>();
        }

        public async Task<ICollection<TEntity>> FindAsync(ISpec<TEntity> spec, CancellationToken token = default(CancellationToken))
        {
            var query = this.SetSpec(spec);
            return await query.ToListAsync(token).ConfigureAwait(false);
        }

        public async Task<TEntity?> FindByIdAsync<TId>(TId id, CancellationToken token = default(CancellationToken))
        {
            return await _dbset.FindAsync(new object[] { id }, token).ConfigureAwait(false);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken))
        {
            return await this._dbset.FirstOrDefaultAsync(predicate, token).ConfigureAwait(false);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(ISpec<TEntity> spec, CancellationToken token = default(CancellationToken))
        {
            var query = this.SetSpec(spec);
            return await query.FirstOrDefaultAsync(token).ConfigureAwait(false);
        }

        public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this._dbset.FirstOrDefault(predicate);
        }

        private IQueryable<TEntity> SetSpec(ISpec<TEntity> specification)
        {
            return SpecHandler<TEntity>.GetQuery(this._dbset.AsQueryable(), specification);
        }

        public async Task InsertAsync(TEntity entity, CancellationToken token = default(CancellationToken))
        {
            await _dbset.AddAsync(entity, token).ConfigureAwait(false);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken token = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                _dbset.Update(entity);
            }).ConfigureAwait(false);
        }

        public async Task DeleteAsync<TId>(TId id, CancellationToken token = default(CancellationToken))
        {
            var deletedItem = await FindByIdAsync<TId>(id, token);

            if (deletedItem != null)
            {
                if (deletedItem is ISoftDeletable e)
                {
                    e.Delete();
                    await this.UpdateAsync(deletedItem);
                }
                else
                {
                    await this.RemoveAsync(deletedItem);
                }
            }
        }

        private async Task RemoveAsync(TEntity entity)
        {
            await Task.Run(() => _dbset.Remove(entity)).ConfigureAwait(false);
        }

    }
}

