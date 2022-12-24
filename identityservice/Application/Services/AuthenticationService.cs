using System.Security.Claims;
using Ardalis.GuardClauses;
using AutoMapper;
using identityservice.Application.Contracts;
using identityservice.Domain.UserAggregate;
using identityservice.Domain.UserAggregate.Interfaces;
using identityservice.Domain.UserAggregate.Specs;
using identityservice.Infrastructure.Security;
using Microsoft.Extensions.Options;
using sharedkernel.Interfaces;
using sharedkernel.ServiceResponse;
using sharedsecurity;

namespace identityservice.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashService _hashService;
        private readonly ITokenService _tokenService;
        private readonly ILogService<AuthenticationService> _logService;
        private readonly IMapper _mapper;
        private readonly JwtConfig _jwtConfig;

        public AuthenticationService(IUnitOfWork unitOfWork, IHashService hashService, ITokenService tokenService, ILogService<AuthenticationService> logService, IMapper mapper, IOptionsMonitor<JwtConfig> jwtConfig)
        {
            _unitOfWork = unitOfWork;
            _hashService = hashService;
            _tokenService = tokenService;
            _logService = logService;
            _mapper = mapper;
            this._jwtConfig = jwtConfig.CurrentValue;           
        }

        public async Task<IServiceResponse> Register(UserRegisterDto userRegisterDto)
        {
            Guard.Against.Null(userRegisterDto, "register", "user could not be null.");

            var checkedUser = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email.EmailAddress == userRegisterDto.Email).ConfigureAwait(false);

            if (checkedUser != null)
                throw new ArgumentException("User already exists.");

            var defaultRole = await this._unitOfWork.RoleRepository.FirstOrDefaultAsync(x => x.IsDefault == true).ConfigureAwait(false);
            Guard.Against.Null(defaultRole, "role", "Role could not be null.");

            var hashPassword = await this._hashService.GetHashedStringAsync(userRegisterDto.Password).ConfigureAwait(false);
            Guard.Against.NullOrWhiteSpace(hashPassword, "hashpassword", "Hash could not be created.");

            var user = User.CreateUser(userRegisterDto.Name, userRegisterDto.Surname, userRegisterDto.Email, hashPassword, defaultRole.Id);

            await this._unitOfWork.UserRepository.InsertAsync(user).ConfigureAwait(false);
            await this._unitOfWork.SaveAsync();

            return ServiceResponse.Success("User was created successfully.");
        }

        public async Task<IServiceResponse<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
        {
            Guard.Against.Null(loginRequestDto, "login request", "Login datas could not be null.");

            var hashPassword = await this._hashService.GetHashedStringAsync(loginRequestDto.Password).ConfigureAwait(false);
            Guard.Against.NullOrWhiteSpace(hashPassword, "hashpassword", "Hash could not be created.");

            var user = await this._unitOfWork.UserRepository.FirstOrDefaultAsync(new GetLoginUserSpec(loginRequestDto.Email, hashPassword)).ConfigureAwait(false);

            Guard.Against.Null(user, "login user", "User could not be found.");

            var response = CreateLoginResponse(user);

            var token = RefreshToken.CreateRefreshToken(
                response.RefreshToken,
                user.Id,
                DateTime.Now.AddHours(_jwtConfig.RefreshTokenDuration.GetValueOrDefault(24)));

            await this._unitOfWork.RefreshTokenRepository.InsertAsync(token).ConfigureAwait(false);
            await this._unitOfWork.SaveAsync();

            return ServiceResponse<LoginResponseDto>.Success(response);
        }

        public async Task<LoginResponseDto> RefreshAccessToken(RefreshAccessTokenDto refreshAccessTokenDto)
        {

            Guard.Against.Null(refreshAccessTokenDto, "refresh token", "RefreshToken request could not be null.");

            ClaimsPrincipal claimsPrincipal = _tokenService.GetTokenClaimPrincipal(refreshAccessTokenDto.AccessToken);
            if (claimsPrincipal == null) throw new ArgumentNullException("Claims could not be found.");

            var userId = claimsPrincipal.Claims.First(c => c.Type == "UId").Value;
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException("Claim principal could not be found.");

            var user = await this._unitOfWork.UserRepository.FindByIdAsync<Guid>(Guid.Parse(userId));
            if (user == null) throw new ArgumentException("User is null");

            var token = await this._unitOfWork.RefreshTokenRepository.FirstOrDefaultAsync(new GetRefreshTokenByUserSpec(user.Id, refreshAccessTokenDto.RefreshToken)).ConfigureAwait(false);
            if (token == null) throw new ArgumentException("Token could not be found.");

            var response = CreateLoginResponse(user);

            var newtoken = RefreshToken.CreateRefreshToken(
                 response.RefreshToken,
                 user.Id,
                 DateTime.Now.AddHours(_jwtConfig.RefreshTokenDuration.GetValueOrDefault(24)));

            await this._unitOfWork.RefreshTokenRepository.DeleteAsync<Guid>(token.Id);
            await this._unitOfWork.RefreshTokenRepository.InsertAsync(newtoken).ConfigureAwait(false);
            await this._unitOfWork.SaveAsync();

            return response;
        }

        private LoginResponseDto CreateLoginResponse(User user)
        {
            var claims = CreateClaimsByUser(user);

            var response = new LoginResponseDto()
            {
                Email = user.Email.EmailAddress,
                AccessToken = _tokenService.GenerateToken(claims),
                RefreshToken = _tokenService.GenerateRefreshToken()
            };

            return response;
        }

        private Claim[] CreateClaimsByUser(User user)
        {
            var claims = new Claim[]
               {
                new Claim("UId", user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Email.EmailAddress),
                new Claim(ClaimTypes.Role,user.Role.Name)
               };

            return claims;

        }

    }

}

