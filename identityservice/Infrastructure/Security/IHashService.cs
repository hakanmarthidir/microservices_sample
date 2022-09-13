namespace identityservice.Infrastructure.Security
{
    public interface IHashService
    {
        string GetHashedString(string hashValue);
        Task<string> GetHashedStringAsync(string hashValue);
        Task<bool> VerifyHashesAsync(string actualValue, string hashedValue);
    }
}

