namespace identityservice.Application.Contracts
{
    public class RefreshAccessTokenDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}

