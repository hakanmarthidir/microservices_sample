using Ardalis.GuardClauses;

namespace reviewservice.Extensions
{
    public static class GuardClausesExtensions
    {
        public static byte ByteNegative(this IGuardClause guardClause, byte input, string message)
        {
            if (input < 0)
                throw new ArgumentException(message);

            return input;
        }
    }
}
