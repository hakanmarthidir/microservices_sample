﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using identityservice.Application.Contracts;
using identityservice.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace identityservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authenticationService.Login(request).ConfigureAwait(false);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
        {
            var result = await _authenticationService.Register(request).ConfigureAwait(false);

            return Ok(result);
        }


        [Authorize(Policy = "AuthorizedClient")]
        [HttpPost("refreshtoken")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshAccessTokenDto request)
        {
            var result = await _authenticationService.RefreshAccessToken(request);

            return Ok(result);
        }
    }
}
