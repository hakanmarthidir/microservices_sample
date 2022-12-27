using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using identityservice.Application.Contracts;
using identityservice.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sharedkernel.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace identityservice.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAuthenticationService authenticationService, ILogger<AccountController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(IServiceResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IServiceResponse))]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            this._logger.LogCritical("account login request.");
            var result = await _authenticationService.Login(request).ConfigureAwait(false);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
        {
            var result = await _authenticationService.Register(request).ConfigureAwait(false);

            return Ok(result);
        }


        [Authorize(Policy = "AuthorizedClient")]
        [HttpPost("refreshtoken")]
        [Route("[action]")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshAccessTokenDto request)
        {
            var result = await _authenticationService.RefreshAccessToken(request);

            return Ok(result);
        }
    }
}

