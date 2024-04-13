using CRUD.implementation;
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
        public string GenerateToken(Employee employee, string role)
        {
            Claim[] claims = [
                new("userId", employee.Id.ToString()),
                new("role", role)
                ];

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

        public string GenerateToken(Manager manager, string role)
        {
            Claim[] claims = [
                new("userId", manager.Id.ToString()),
                new("role", role)
                ];

            SigningCredentials signInCreadentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key)),
                SecurityAlgorithms.HmacSha256
                );

            JwtSecurityToken securityToken = new JwtSecurityToken(
                signingCredentials: signInCreadentials,
                expires: DateTime.UtcNow.AddHours(12),
                claims: claims
                );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken).ToString();

            return token;
        }

        public string GenerateToken(Admin admin, string role)
        {
            Claim[] claims = [
                new("userId", admin.Id.ToString()),
                new("role", role)
                ];

            SigningCredentials signInCreadentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key)),
                SecurityAlgorithms.HmacSha256
                );

            JwtSecurityToken securityToken = new JwtSecurityToken(
                signingCredentials: signInCreadentials,
                expires: DateTime.UtcNow.AddHours(12),
                claims: claims
                );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken).ToString();

            return token;
        }

        public string GenerateToken(Company company, string role)
        {
            Claim[] claims = [
                new("companyId", company.Id.ToString()),
                new("role", role)
                ];

            SigningCredentials signInCreadentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret_key)),
                SecurityAlgorithms.HmacSha256
                );

            JwtSecurityToken securityToken = new JwtSecurityToken(
                signingCredentials: signInCreadentials,
                expires: DateTime.UtcNow.AddHours(12),
                claims: claims
                );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken).ToString();

            return token;
        }
    }
}
