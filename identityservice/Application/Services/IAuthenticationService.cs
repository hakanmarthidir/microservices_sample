using System;
using identityservice.Application.Contracts;
using sharedkernel.Interfaces;

namespace identityservice.Application.Services
{
    public interface IAuthenticationService
    {
        Task<IServiceResponse> Register(UserRegisterDto userRegisterDto);
        Task<IServiceResponse<LoginResponseDto>> Login(LoginRequestDto loginRequestDto);
        Task<LoginResponseDto> RefreshAccessToken(RefreshAccessTokenDto refreshAccessTokenDto);
    }

}

