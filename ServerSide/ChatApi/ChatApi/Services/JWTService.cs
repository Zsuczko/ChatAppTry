using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApi.Services
{
    public class JWTService
    {

        private readonly IConfiguration _config;

        public JWTService(IConfiguration config) { 
        
            _config = config;
        }

        public string GenerateToken(string username) {


            var claims = new[] {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my-super-secret-key111111111111111111111111111111111"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer:"localhost",
                audience:"localhost",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(60)),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
