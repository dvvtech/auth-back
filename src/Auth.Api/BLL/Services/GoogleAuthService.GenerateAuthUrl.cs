using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Oauth2.v2;

namespace Auth.Api.BLL.Services
{
    public partial class GoogleAuthService
    {
        /// <summary>
        /// Возвращаем пользователю урл для авторизации в гугл
        /// </summary>
        /// <returns></returns>
        public string GenerateAuthUrl(string domainName)
        {
            if (_authConfig.Sites.TryGetValue(domainName, out var config))
            {
                _logger.LogInformation(config.ClientId);

                return new GoogleAuthorizationCodeFlow(
                    new GoogleAuthorizationCodeFlow.Initializer
                    {
                        ClientSecrets = GetClientSecrets(config.ClientId, config.ClientSecret),
                        Scopes = GetScopes(),
                        Prompt = "consent"
                    }).CreateAuthorizationCodeRequest(config.RedirectUrl).Build().ToString();
            }
            else
            {
                return "";
            }            
        }

        private ClientSecrets GetClientSecrets(string clientId, string clientSecret)
        {            
            return new() { ClientId = clientId, ClientSecret = clientSecret };
        }

        private string[] GetScopes()
        {
            return new[]
            {
                Oauth2Service.Scope.Openid,
                Oauth2Service.Scope.UserinfoEmail,
                Oauth2Service.Scope.UserinfoProfile,
            };
        }
    }
}
