using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SignalR_Authentication.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace SignalR_Authentication.Models
{
    public class TokenService : ITokenService
    {
        public static readonly string Key = "@#$%^&*()_)ASDFGHJKL:CVBNM<>?";
        public static readonly string Issuer = "Surman";
        public static readonly string Audience = "Nobody";

        public string GenerateToken(LoginModel model)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, model.Name),
                new Claim(ClaimTypes.Role, model.Role),
                new Claim(ClaimTypes.Email, model.Email)
            };

            ClaimsIdentity identity = new ClaimsIdentity
                (claims, JwtBearerDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);

            SigningCredentials credentials = 
                new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key)), SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = tokenHandler.CreateJwtSecurityToken
                (
                    issuer:Issuer,
                    audience:Audience,
                    subject:identity,
                    expires:DateTime.Now.AddMinutes(1),
                    signingCredentials:credentials
                );

            return tokenHandler.WriteToken(jwtToken);
        }

        public bool ValidateToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                handler.ValidateToken(token, new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = Issuer,
                    ValidAudience = Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key))

                }, out SecurityToken securityToken);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
