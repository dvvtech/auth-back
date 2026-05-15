using Auth.Api.BLL.Abstract;
using Auth.Api.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Auth.Api.BLL.Domain;

namespace Auth.Api.BLL.Services
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtConfig _options;

        public JwtProvider(IOptions<JwtConfig> options)
        {
            _options = options.Value;
        }

        public string GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Genetate jwt access token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GenerateToken(string userName, int accountId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, accountId.ToString()),
                new Claim(ClaimTypes.Role, Role.User.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new JwtSecurityToken(_options.Issuer,
                                                       _options.Audience,
                                                       claims,
                                                       expires: DateTime.Now.Add(_options.ExpiresDuration),
                                                       signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }        
    }
}
