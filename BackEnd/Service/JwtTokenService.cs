using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Models.Classes;
//using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace itb2203_2024_predictiongame.Service
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]!));
        }

        public string GenerateToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName!)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        // public ClaimsPrincipal? ValidateToken(string token)
        // {
        //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        //     var tokenHandler = new JwtSecurityTokenHandler();

        //     try
        //     {
        //         var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
        //         {
        //             ValidateIssuerSigningKey = true,
        //             IssuerSigningKey = key,
        //             ValidateIssuer = true,
        //             ValidateAudience = true,
        //             ValidIssuer = _issuer,
        //             ValidAudience = _audience,
        //             ClockSkew = TimeSpan.Zero // Reduce the default clock skew
        //         }, out _);

        //         return claimsPrincipal;
        //     }
        //     catch
        //     {
        //         return null; // Token validation failed
        //     }
        // }
    }
}