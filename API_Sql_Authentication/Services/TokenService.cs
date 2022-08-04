using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_Sql_Authentication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace API_Sql_Authentication.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(TestUser validatedUser)
        {
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim("Id", validatedUser.Id.ToString()),
                        new Claim("FirstName",validatedUser.FirstName),
                        new Claim("LastName",validatedUser.LastName),
                        new Claim("Email",validatedUser.Email),
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<int> GetUserIdFromToken(HttpContext context)
        {
            var accesstoken = await context.GetTokenAsync("access_token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(accesstoken) as JwtSecurityToken;
            return Int32.Parse(jwtToken.Claims.First(claim => claim.Type == "Id").Value);
        }



    }
}