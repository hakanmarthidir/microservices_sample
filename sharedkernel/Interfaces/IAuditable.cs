namespace sharedkernel.Interfaces
{
    public interface IAuditable
    {
        string? CreatedBy { get; set; }
        DateTimeOffset? CreatedDate { get; set; }
        string? LastModifiedBy { get; set; }
        DateTimeOffset? LastModifiedDate { get; set; }
    }
}

