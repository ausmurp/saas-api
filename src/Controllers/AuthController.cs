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
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var user = _authService.Authenticate(request);

            var useCookie = CookieAuthenticationDefaults.AuthenticationScheme.Equals(
                Request.Headers["auth-scheme"], StringComparison.CurrentCultureIgnoreCase);

            var tokenExpDate = DateTime.UtcNow.AddDays(7);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var authScheme = useCookie ? CookieAuthenticationDefaults.AuthenticationScheme : JwtBearerDefaults.AuthenticationScheme;

            var claimId = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
            }, authScheme);

            // Create the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimId,
                Expires = tokenExpDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Add the cookie
            if (useCookie)
            {
                var tokenClaimId = new ClaimsIdentity(tokenDescriptor.Subject);
                tokenClaimId.AddClaim(new Claim(ClaimTypes.Authentication, tokenString));

                var authProperties = new AuthenticationProperties()
                {
                    IsPersistent = request.RememberDevice,
                };

                var claimPrin = new ClaimsPrincipal(tokenClaimId);
                await this.HttpContext.SignInAsync(claimPrin, authProperties);
            }

            // return basic user info and authentication token
            // At this point the bearer token or cookie can be used for auth
            var resp = new LoginResponse
            {
                User = user
            };

            if (!useCookie)
            {
                resp.Session = new AuthSession
                {
                    AccessToken = tokenString,
                    TokenType = "Bearer",
                    ExpiresIn = tokenExpDate.Ticks
                };
            }

            return Ok(resp);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterRequest request)
        {
            _authService.Register(request);
            return Ok();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutCookie()
        {
            var authScheme = HttpContext.User.Identity.AuthenticationType ?? "";

            var isCookie = CookieAuthenticationDefaults.AuthenticationScheme.Equals(authScheme, StringComparison.CurrentCultureIgnoreCase);

            if (isCookie)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            // Client is responsible for deleting JWT

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
