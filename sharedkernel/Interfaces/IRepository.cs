namespace sharedkernel.Interfaces
{
    public interface IRepository<TEntity> : IQueryRepository<TEntity>, ICommandRepository<TEntity> where TEntity : class, IEntity
    {

    }
}

