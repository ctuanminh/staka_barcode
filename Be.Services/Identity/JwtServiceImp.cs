using Be.Common.Dtos.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Be.Services.Identity
{
    public class JwtServiceImp : IJwtService
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly ILogger<JwtServiceImp> _logger;
        private readonly JwtOptions _options;

        public JwtServiceImp(ILogger<JwtServiceImp> logger, IOptions<JwtOptions> options)
        {
            _jwtSecurityTokenHandler ??= new JwtSecurityTokenHandler();
            _logger = logger;
            _options = options.Value;
        }

        public string GenerateAccessToken(long userId, string email, List<string> roles)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email)
         };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_options.ExpiresInMinutes),
                SigningCredentials = signingCredentials
            };

            return _jwtSecurityTokenHandler.WriteToken(_jwtSecurityTokenHandler.CreateToken(tokenDescriptor));
        }

        public string GenerateAccessToken(long userId, string email, List<string> roles, string phoneNumber)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.MobilePhone, phoneNumber),
                new Claim(ClaimTypes.Email, email),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_options.ExpiresInMinutes),
                SigningCredentials = signingCredentials
            };

            return _jwtSecurityTokenHandler.WriteToken(_jwtSecurityTokenHandler.CreateToken(tokenDescriptor));
        }


        public string GenerateRefreshToken()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();

            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifeTime = true)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                    ValidateLifetime = validateLifeTime
                };

                var principal = _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch (Exception e)
            {
                _logger.LogError("Token validation failed: {@e}", e);

                return null;
            }
        }
    }
}