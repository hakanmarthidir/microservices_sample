namespace identityservice.Application.Contracts
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}

