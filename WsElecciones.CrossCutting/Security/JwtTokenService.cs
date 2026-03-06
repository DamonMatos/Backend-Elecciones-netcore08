using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WsElecciones.Domain.Entities;
using Microsoft.Extensions.Configuration;
using WsElecciones.Domain.Interface;
using WsElecciones.Domain.Views.Auth;

namespace WsElecciones.CrossCutting.Security;
    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly SymmetricSecurityKey _signingKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryMinutes;

        public JwtTokenService(IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("Jwt");

            var rawKey = jwtSection["Key"]
                ?? throw new InvalidOperationException("JWT Key no configurada. Revisa appsettings o variables de entorno.");

            if (rawKey.Length < 32)
                throw new InvalidOperationException("JWT Key debe tener al menos 32 caracteres.");

            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(rawKey));
            _issuer = jwtSection["Issuer"] ?? "WsElecciones";
            _audience = jwtSection["Audience"] ?? "WsEleccionesClients";
            _expiryMinutes = int.TryParse(jwtSection["ExpiryMinutes"], out var minutes) ? minutes : 60;
        }

        public (string Token, DateTime Expiry) GenerateToken(UserDto user)
        {
            var now = DateTime.UtcNow;
            var expiry = now.AddMinutes(_expiryMinutes);
            //var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
            //var nowPeru = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, peruTimeZone);
            //var expiry = nowPeru.AddMinutes(_expiryMinutes);

            var claims = new[]
                        {
                        new Claim(JwtRegisteredClaimNames.Sub,         user.IdUsuario.ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName,  user.NomPer.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email,       user.Correo.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti,         Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,
                                  DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                                  ClaimValueTypes.Integer64),
                        new Claim(ClaimTypes.Role, user.Perfil)
                    };

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiry,
                signingCredentials: new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
            );

            return (new JwtSecurityTokenHandler().WriteToken(tokenDescriptor), expiry);
        }
}

