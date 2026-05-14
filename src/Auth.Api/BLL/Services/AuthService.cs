using Auth.Api.BLL.Abstract;
using Auth.Api.DAL;
using Auth.Api.DAL.Entities;
using Auth.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthDbContext _dbContext;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(
            AuthDbContext dbContext,
            IJwtProvider jwtProvider)
        {
            _dbContext = dbContext;
            _jwtProvider = jwtProvider;
        }

        public async Task LogoutAsync(int accountId)
        {
            AccountEntity accountEntity = await _dbContext.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(account => account.Id == accountId);

            if (accountEntity is not null)
            {
                accountEntity.JwtRefreshToken = string.Empty;

                await _dbContext.Accounts
                                    .Where(updateUser => updateUser.Id == accountEntity.Id)
                                    .ExecuteUpdateAsync(updateUser => updateUser
                                        .SetProperty(c => c.JwtRefreshToken, accountEntity.JwtRefreshToken)
                                        .SetProperty(c => c.UpdateUtcDate, DateTime.UtcNow));
            }
        }

        public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
        {
            AccountEntity accountEntity = await _dbContext.Accounts
                .AsNoTracking()
                .FirstOrDefaultAsync(account => account.JwtRefreshToken == refreshToken);

            if (accountEntity == null || accountEntity.IsBlocked)
            {
                return null;
            }

            var newAccessToken = _jwtProvider.GenerateToken(accountEntity.UserName, accountEntity.Id);
            var newRefreshToken = _jwtProvider.GenerateRefreshToken();

            accountEntity.JwtRefreshToken = newRefreshToken;

            await _dbContext.Accounts
                                    .Where(updateUser => updateUser.Id == accountEntity.Id)
                                    .ExecuteUpdateAsync(updateUser => updateUser
                                        .SetProperty(c => c.JwtRefreshToken, accountEntity.JwtRefreshToken)
                                        .SetProperty(c => c.UpdateUtcDate, DateTime.UtcNow));

            return new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
