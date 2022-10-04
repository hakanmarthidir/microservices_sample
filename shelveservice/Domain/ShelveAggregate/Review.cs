using sharedkernel;

namespace shelveservice.Domain.ShelveAggregate
{
    public class Review : BaseEntity<Guid>
    {
        public Guid BookId { get; private set; }
        public DateTime? DateRead { get; private set; }
        public byte Rating { get; private set; }
        public string Comment { get; private set; }
    }

}

