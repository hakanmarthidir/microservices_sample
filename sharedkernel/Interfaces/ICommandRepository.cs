namespace sharedkernel.Interfaces
{
    public interface ICommandRepository<TEntity> where TEntity : class, IEntity
    {
        Task InsertAsync(TEntity entity, CancellationToken token = default(CancellationToken));
        Task UpdateAsync(TEntity entity, CancellationToken token = default(CancellationToken));
        Task DeleteAsync<TId>(TId id, CancellationToken token = default(CancellationToken));
    }
}

