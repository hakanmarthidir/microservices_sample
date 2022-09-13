using System.Linq.Expressions;

namespace sharedkernel.Interfaces
{
    public interface IQueryRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> FindByIdAsync<TId>(TId id, CancellationToken token = default(CancellationToken));
        Task<ICollection<TEntity>> FindAsync(ISpec<TEntity> spec, CancellationToken token = default(CancellationToken));
        Task<TEntity?> FirstOrDefaultAsync(ISpec<TEntity> spec, CancellationToken token = default(CancellationToken));
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default(CancellationToken));
    }
}

