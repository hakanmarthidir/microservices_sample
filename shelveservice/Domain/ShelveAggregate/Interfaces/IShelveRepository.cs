using shelveservice.Domain.ShelveAggregate;
using sharedkernel.Interfaces;

namespace shelveservice.Domain.ShelveAggregate.Interfaces
{
    public interface IShelveRepository : IQueryRepository<Shelve>, ICommandRepository<Shelve>
    {
    }
}

