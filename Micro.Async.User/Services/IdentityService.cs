using Micro.Async.User.Contracts.User.Request;
using Micro.Async.User.Contracts.User.Response;
using Micro.Async.User.Interfaces;
using Micro.Async.User.Persistance;
using Micro.Async.User.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Micro.Async.User.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<Micro.Async.User.Models.User> _userManager;
        private readonly UserDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(UserManager<Micro.Async.User.Models.User> userManager, UserDbContext context, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _context = context;
            _jwtSettings = jwtSettings;
        }
        public async Task<AuthenticationResponse> Login(LoginRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser == null)
            {
                return new AuthenticationResponse
                {
                    StatusCode = 400,
                    Error = "User with this email address does not exist"
                };
            }
            var result = await _userManager.CheckPasswordAsync(existingUser, request.Password);
            if (!result)
            {
                return new AuthenticationResponse
                {
                    StatusCode = 400,
                    Error = "Invalid password"
                };
            }

            var roleList = await _userManager.GetRolesAsync(existingUser);
            var role = roleList.FirstOrDefault();
            var token = GenerateToken(existingUser.Id, role);


            return new AuthenticationResponse
            {
                StatusCode = 200,
                Token = token,
                User = existingUser,
                Role = role
            };
        }

        public async Task<AuthenticationResponse> Register(Micro.Async.User.Models.User request, string password)
        {

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthenticationResponse
                {
                    StatusCode = 400,
                    Error = "User with this email address already exists"
                };
            }

            var newUser = await _userManager.CreateAsync(request, password);
            if (!newUser.Succeeded)
            {
                return new AuthenticationResponse
                {
                    StatusCode = 500,
                    Error = "Internal server error"
                };
            }

            await _userManager.AddToRoleAsync(request, "User");

            var token = GenerateToken(request.Id, "User");
            return new AuthenticationResponse
            {
                StatusCode = 200,
                Token = token,
                User = request,
                Role = "User"
            };


        }

        private string GenerateToken(int id, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", id.ToString()),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(200),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
