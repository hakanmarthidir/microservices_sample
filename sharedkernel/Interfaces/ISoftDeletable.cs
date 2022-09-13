namespace sharedkernel.Interfaces
{
    public interface ISoftDeletable
    {
        string? DeletedBy { get; set; }
        DateTimeOffset? DeletedDate { get; set; }
        Status Status { get; set; }
        void Delete();
    }
}

