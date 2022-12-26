using sharedkernel;

namespace shelveservice.Domain.ShelveAggregate
{
    public class ShelvedBook : BaseEntity<Guid>
    {
        public Guid BookId { get; private set; }
        public DateTime DateAdd { get; private set; }
        public Guid ShelveId { get; private set; }
        public Shelve Shelve { get; private set; }
    }

}

