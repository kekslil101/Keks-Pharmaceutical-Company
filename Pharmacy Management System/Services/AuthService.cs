using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pharmacy_Management_System.Data;
using Pharmacy_Management_System.Models;
using Pharmacy_Management_System.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pharmacy_Management_System.Services
{
    public class AuthService : IAuthService
    {
        private readonly PharmacyDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(PharmacyDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Agents.AnyAsync(a => a.Phone == dto.Phone))
                return "Phone already registered";

            var agent = new Agent
            {
                Name = dto.Name,
                Age = dto.Age,
                Phone = dto.Phone,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Gender = dto.Gender,
                Role = dto.Role ?? "Agent" // Default to Agent
            };

            _context.Agents.Add(agent);
            await _context.SaveChangesAsync();
            return "Registration successful";
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var agent = await _context.Agents.FirstOrDefaultAsync(a => a.Phone == dto.Phone);
            if (agent == null || !BCrypt.Net.BCrypt.Verify(dto.Password, agent.Password))
                return "Invalid credentials";

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, agent.Phone),
                new Claim(ClaimTypes.NameIdentifier, agent.Id.ToString()),
                new Claim(ClaimTypes.Role, agent.Role) // Role claim
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
