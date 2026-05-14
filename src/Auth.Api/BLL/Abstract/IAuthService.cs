using Auth.Api.Models;

namespace Auth.Api.BLL.Abstract
{
    public interface IAuthService
    {
        Task LogoutAsync(int accountId);

        Task<TokenResponse> RefreshTokenAsync(string refreshToken);
    }
}
