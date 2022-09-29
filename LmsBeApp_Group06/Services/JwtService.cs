using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LmsBeApp_Group06.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LmsBeApp_Group06.Services
{
    public class JwtService
    {
        public string GenerateToken(Claim[] additionalClaims, string secretKey, int TimeLife)
        {
            var claims = new List<Claim>();
            claims.AddRange(additionalClaims);

            byte[] key = Convert.FromBase64String(secretKey);
            var securityKey = new SymmetricSecurityKey(key);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(TimeLife),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)

            };

            var jwtSecurityHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtSecurityHandler.CreateJwtSecurityToken(descriptor);
            return jwtSecurityHandler.WriteToken(jwtToken);
        }

        public ClaimsPrincipal GetClaimsPrincipal(string token, string recretKey)
        {
            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = handler.ReadJwtToken(token);
                if (token == null)
                {
                    return null;
                }
                byte[] key = Convert.FromBase64String(recretKey);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal claimsPrincipal = handler.ValidateToken(token, parameters, out securityToken);
                return claimsPrincipal;
            }
            catch
            {
                return null;
            }
        }
    }
}