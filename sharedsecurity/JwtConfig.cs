using System;
namespace sharedsecurity
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public double? Duration { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double? RefreshTokenDuration { get; set; }
    }
}

