using Auth.Api.BLL.Abstract;
using Auth.Api.Extensions;
using Auth.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthorizeController> _logger;

        public AuthorizeController(IAuthService authService,
                                   ILogger<AuthorizeController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("logout"), Authorize]
        public async Task<ActionResult> Logout()
        { 
            var accountId = this.GetCurrentAccountId();
            if (accountId.HasValue)
            {
                await _authService.LogoutAsync(accountId.Value);
            }
            else 
            {
                return BadRequest("fail logout");
            }

            ClearTokenCookie("accessToken");
            ClearTokenCookie("refreshToken");

            return Ok();
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponse>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("refresh token not found");
            }

            TokenResponse tokenResult = await _authService.RefreshTokenAsync(refreshToken);
            if (tokenResult == null)
            {
                return BadRequest("fail refresh-token");
            }

            SetTokenCookie("accessToken", tokenResult.AccessToken, SameSiteMode.None);
            SetTokenCookie("refreshToken", tokenResult.RefreshToken, SameSiteMode.Strict);

            return Ok();
        }

        [HttpGet("test")]
        public ActionResult Test()
        {
            var domainOnly = Request.Host.Host;

            return Ok("123" + domainOnly);
        }

        private void SetTokenCookie(string name, string value, SameSiteMode sameSite)
        {
            Response.Cookies.Append(name, value, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = sameSite,
                Domain = ".bacbac.ru",
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });
        }

        private void ClearTokenCookie(string name)
        {
            Response.Cookies.Delete(name, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Domain = ".bacbac.ru",
                Path = "/"
            });
        }
    }
}
