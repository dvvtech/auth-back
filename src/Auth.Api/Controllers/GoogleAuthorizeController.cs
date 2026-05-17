using Auth.Api.BLL.Services;
using Auth.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("google-auth")]
    [ApiController]
    public class GoogleAuthorizeController : ControllerBase
    {
        private readonly GoogleAuthService _authService;
        private readonly ILogger<GoogleAuthorizeController> _logger;

        public GoogleAuthorizeController(
            GoogleAuthService googleAuthService,
            ILogger<GoogleAuthorizeController> logger)
        {
            _authService = googleAuthService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Authorize()
        {            
            var domainName = Request.Host.Host;
            _logger.LogInformation(domainName);
            var authUrl = _authService.GenerateAuthUrl(domainName);
            return Ok(authUrl);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest("Google auth: Code is required");
                }

                var domainName = Request.Host.Host;                
                TokenResponse tokenResponse = await _authService.HandleCallback(code, domainName);

                SetTokenCookie("accessToken", tokenResponse.AccessToken, sameSite: SameSiteMode.None);
                SetTokenCookie("refreshToken", tokenResponse.RefreshToken, sameSite: SameSiteMode.Strict);

                return Redirect("https://bacbac.ru");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Google auth exception: {ex.Message}");
                return BadRequest(ex.Message);
            }
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
    }
}
