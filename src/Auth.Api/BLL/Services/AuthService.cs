using Auth.Api.BLL.Abstract;
using Auth.Api.DAL;
using Auth.Api.Models;

namespace Auth.Api.BLL.Services
{
    public class AuthService : IAuthService
    {
        public AuthService(AuthDbContext authDbContext)
        {
            
        }

        public Task LogoutAsync(int accountId)
        {
            throw new NotImplementedException();
        }

        public Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
