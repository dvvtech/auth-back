using Auth.Api.BLL.Abstract;
using Auth.Api.Extensions;
using Auth.Api.Models;
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

            return Ok();
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponse>> RefreshToken(RefreshTokenRequest request)
        {            
            TokenResponse tokenResult = await _authService.RefreshTokenAsync(request.RefreshToken);
            if (tokenResult == null)
            {
                return BadRequest("fail refresh-token");
            }

            return Ok(tokenResult);
        }

        [HttpGet("test")]
        public ActionResult Test()
        {
            var domainOnly = Request.Host.Host;

            return Ok("123" + domainOnly);
        }
    }
}
