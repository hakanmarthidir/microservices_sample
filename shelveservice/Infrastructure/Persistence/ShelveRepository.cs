using shelveservice.Domain.ShelveAggregate;
using shelveservice.Domain.ShelveAggregate.Interfaces;
using shelveservice.Infrastructure.Persistence;
using sharedkernel;

namespace shelveservice.Infrastructure.Persistence
{
    public class ShelveRepository : RepositoryBase<ShelveContext, Shelve>, IShelveRepository
    {
        public ShelveRepository(ShelveContext context) : base(context)
        {
        }
    }
}

