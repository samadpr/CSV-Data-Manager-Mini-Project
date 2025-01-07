using Domain.Services.Login.DTOs;
using Domain.Services.Login.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Login
{
    public class LoginRequestService : ILoginRequestService
    {
        private readonly ILoginRequestRepository _loginRequestRepository;
        private readonly IConfiguration _configuration;
        public LoginRequestService(ILoginRequestRepository loginRequestRepository, IConfiguration configuration)
        {
            _loginRequestRepository = loginRequestRepository;
            _configuration = configuration;
        }
        public async Task<LoginResponseDto?> AuthenticateUserAsync(LoginRequestDto loginRequest)
        {
            try
            {
                var user = await _loginRequestRepository.GetUserByEmailAndPasswordAsync(loginRequest);
                if (user == null) return null;


                var token = GenerateJwtToken(user.Email, user.Id.ToString(), user.Role);
                return new LoginResponseDto { Token = token, userId = user.Id };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AuthenticateUserAsync: {ex.Message}");
                return null;
            }

        }

        private string GenerateJwtToken(string email, string userId, string role)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(userId))
                    throw new ArgumentNullException(nameof(email) + " " + nameof(userId), "email and userId cannot be null or empty");

                if (string.IsNullOrEmpty(role))
                    throw new ArgumentNullException(nameof(role), "role cannot be null or empty");

                // Get the key from configuration
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var signIn = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

                // Create claims
                var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", userId),
            new Claim("Email", email),
            new Claim("Role", role)
        };

                // Get the expiration time from configuration
                int expiresInHours = Convert.ToInt32(_configuration["Jwt:ExpiresInHours"] ?? "1");

                // Create the JWT token
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(expiresInHours),
                    signingCredentials: signIn
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // Log the error (replace with a logger in production)
                Console.WriteLine($"Error in GenerateJwtToken: {ex.Message}");
                throw;
            }
        }

    }
}
