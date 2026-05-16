using Auth.Api.DAL.Entities;
using Auth.Api.Models;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace Auth.Api.BLL.Services
{
    public partial class GoogleAuthService
    {
        /// <summary>
        /// После авторизации пользователя гугл вызовет этот код и отправит code и мы его поменяем на токен
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<TokenResponse> HandleCallback(string code)
        {
            try
            {
                var flow = new GoogleAuthorizationCodeFlow(
                    new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = GetClientSecrets(),
                        Scopes = GetScopes()
                    });

                //var tokenResponse = await flow.ExchangeCodeForTokenAsync("user", code, _authConfig.RedirectUrl, CancellationToken.None);
                var tokenResponse = await flow.ExchangeCodeForTokenAsync("user", code, "_authConfig.RedirectUrl", CancellationToken.None);
                GoogleUserInfo userInfo = await GetUserInfo(tokenResponse.AccessToken);

                string accessToken = string.Empty;
                string refreshToken = _jwtProvider.GenerateRefreshToken();
                
                AccountEntity account = await _dbContext
                    .Accounts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(user => user.ExternalId == userInfo.Sub);
                if (account == null)
                {
                    var newAccount = new AccountEntity
                    {
                        ExternalId = userInfo.Sub,
                        UserName = userInfo.GivenName,
                        Email = userInfo.Email,
                        JwtRefreshToken = refreshToken,
                        AuthType = AuthTypeEntity.Google,
                        Role = RoleEntity.User,
                        CreatedUtcDate = DateTime.UtcNow,
                        UpdateUtcDate = DateTime.UtcNow
                    };

                    await _dbContext.Accounts.AddAsync(newAccount);
                    await _dbContext.SaveChangesAsync();

                    //int accountId = await _accountRepository.Add(newUserEntity);
                    accessToken = _jwtProvider.GenerateToken(userInfo.GivenName, newAccount.Id);
                }
                else
                {
                    accessToken = _jwtProvider.GenerateToken(userInfo.GivenName, account.Id);
                    account.JwtRefreshToken = refreshToken;
                    await _dbContext.Accounts
                                    .Where(updateUser => updateUser.Id == account.Id)
                                    .ExecuteUpdateAsync(updateUser => updateUser
                                        .SetProperty(c => c.JwtRefreshToken, account.JwtRefreshToken)
                                        .SetProperty(c => c.UpdateUtcDate, DateTime.UtcNow));                    
                }

                return new TokenResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HandleCallback error");
                return null;
            }
        }

        public async Task<GoogleUserInfo> GetUserInfo(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo");
            response.EnsureSuccessStatusCode();
            var userInfo = await response.Content.ReadFromJsonAsync<GoogleUserInfo>();
            return userInfo;
        }
    }
}
