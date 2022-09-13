using System;
using Ardalis.GuardClauses;
using System.Security.Cryptography;
using System.Text;

namespace identityservice.Infrastructure.Security
{
    public class HashService : IHashService
    {
        public string GetHashedString(string hashValue)
        {
            Guard.Against.NullOrWhiteSpace(hashValue, nameof(hashValue), "Hash value could not be null.");

            string hashedValue = string.Empty;

            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(hashValue);
                byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
                hashedValue = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }

            return hashedValue;
        }

        public async Task<string> GetHashedStringAsync(string hashValue)
        {
            var myHashFunc = new Func<string, string>((hash) =>
            {
                return this.GetHashedString(hash);
            });

            return await Task.Run<string>(() =>
            {
                return myHashFunc(hashValue);
            }).ConfigureAwait(false);
        }

        public async Task<bool> VerifyHashesAsync(string actualValue, string hashedValue)
        {
            var value = await GetHashedStringAsync(actualValue).ConfigureAwait(false);
            var hashed = Guard.Against.NullOrWhiteSpace(hashedValue, nameof(hashedValue), "Hashed value could not be null.");
            return value == hashed;
        }


    }
}

