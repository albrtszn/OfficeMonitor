using DataBase.Repository.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OfficeMonitor.MiddleWares.Authorization
{
    public class JwtProvider
    {
        //todo JWT secret-key
        private string secret_key = "ssseeecccrrreeettt_kkkeeeyyy_12345";
        public string GenerateToken(Employee employee)
        {
            Claim[] claims = [new("employeeId", employee.Id.ToString())];

            SigningCredentials signInCreadentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key)),
                SecurityAlgorithms.HmacSha256
                );

            JwtSecurityToken securityToken = new JwtSecurityToken(
                signingCredentials: signInCreadentials,
                expires: DateTime.UtcNow.AddHours(12),
                claims: claims
                );

            string token = new JwtSecurityTokenHandler().WriteToken( securityToken ).ToString();

            return token;
        }
    }
}
