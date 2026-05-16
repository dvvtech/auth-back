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
            var domainNam11e = Request.Host.Host;
            _logger.LogInformation("1: " + domainNam11e);//
            var domainName = "api.bacbac.ru";
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

                var domainNam11e = Request.Host.Host;
                _logger.LogInformation("2: " + domainNam11e);
                var domainName = "api.bacbac.ru";

                TokenResponse tokenResponse = new TokenResponse();//await _authService.HandleCallback(code, domainName);

                //Перенаправляем пользователя на фронтенд
                return Redirect($"https://somedomain.com?" +
                                $"accessToken={Uri.EscapeDataString(tokenResponse.AccessToken)}&" +
                                $"refreshToken={Uri.EscapeDataString(tokenResponse.RefreshToken)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Google auth exception: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
