using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text;
using OnePrivateNavigation.Common.Models;
using OnePrivateNavigation.Common.Models.Auth;
using OnePrivateNavigation.Data;
using OnePrivateNavigation.Data.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace OnePrivateNavigation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly OnePrivateNavigationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(OnePrivateNavigationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
            {
                return BadRequest(new ApiResponse<LoginResponse>
                {
                    Success = false,
                    Message = "用户名或密码错误"
                });
            }

            if (user.PasswordHash != HashHelper.ComputeSHA256(request.Password + user.Salt))
            {
                return BadRequest(new ApiResponse<LoginResponse>
                {
                    Success = false,
                    Message = "用户名或密码错误"
                });
            }

            var token = GenerateJwtToken(user);

            // 更新最后登录时间
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<LoginResponse>
            {
                Success = true,
                Data = new LoginResponse
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email
                }
            });
        }

        private string GenerateJwtToken(Data.Entities.User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "your-default-key-here"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationInMinutes"]!)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        [Authorize]
        [HttpPut("update-user")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> UpdateUser(UpdateUserRequest request)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid)?.Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user == null)
            {
                return BadRequest(new ApiResponse<LoginResponse>
                {
                    Success = false,
                    Message = "用户不存在"
                });
            }

            if (user.PasswordHash != HashHelper.ComputeSHA256(request.OldPassword))
            {
                return BadRequest(new ApiResponse<LoginResponse>
                {
                    Success = false,
                    Message = "原密码错误"
                });
            }

            if (!string.IsNullOrEmpty(request.NewUsername))
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == request.NewUsername && u.Id != user.Id);
                if (existingUser != null)
                {
                    return BadRequest(new ApiResponse<LoginResponse>
                    {
                        Success = false,
                        Message = "用户名已被使用"
                    });
                }
                user.Username = request.NewUsername;
            }

            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                user.PasswordHash = HashHelper.ComputeSHA256(request.NewPassword);
            }

            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return Ok(new ApiResponse<LoginResponse>
            {
                Success = true,
                Data = new LoginResponse
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email
                }
            });
        }

    }
}