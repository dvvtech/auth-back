using Auth.Api.BLL.Abstract;
using Auth.Api.Configuration;
using Auth.Api.DAL;
using Microsoft.Extensions.Options;

namespace Auth.Api.BLL.Services
{
    public partial class GoogleAuthService
    {
        private readonly GoogleAuthConfig _authConfig;
        private readonly IJwtProvider _jwtProvider;
        private readonly AuthDbContext _dbContext;
        private readonly HttpClient _httpClient;
        private readonly ILogger<GoogleAuthService> _logger;

        public GoogleAuthService(
            IOptions<GoogleAuthConfig> authConfig,
            AuthDbContext dbContext,
            IJwtProvider jwtProvider,
            HttpClient httpClient,
            ILogger<GoogleAuthService> logger)
        {
            _authConfig = authConfig.Value;
            _dbContext = dbContext;
            _jwtProvider = jwtProvider;
            _httpClient = httpClient;
            _logger = logger;
        }
    }
}
