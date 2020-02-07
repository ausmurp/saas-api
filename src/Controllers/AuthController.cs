using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SaaSApi.Data.Entities;
using SaaSApi.Logic.Services;
using SaaSApi.Logic.Models;
using SaaSApi.Common;

namespace SaaSApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AuthController(
            IAuthService authService,
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _authService = authService;
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginRequest request)
        {
            var user = _authService.Authenticate(request);

            var tokenExpDate = DateTime.UtcNow.AddDays(7);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = tokenExpDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new LoginResponse
            {
                Session = new AuthSession
                {
                    AccessToken = tokenString,
                    TokenType = "Bearer",
                    ExpiresIn = tokenExpDate.Ticks
                },
                User = user,
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterRequest request)
        {
            _authService.Register(request);
            return Ok();
        }

        [Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok();
        }


    }
}
