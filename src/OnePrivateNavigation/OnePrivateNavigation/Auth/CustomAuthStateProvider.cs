using OnePrivateNavigation.Data.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnePrivateNavigation.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {

        private readonly IHttpContextAccessor _contextAccessor;

        private AuthenticationState currentUser = new AuthenticationState(new ClaimsPrincipal());

        private readonly IConfiguration _configuration;

        public CustomAuthStateProvider(CustomAuthenticationService service, IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
            service.UserChanged += (newUser) =>
            {
                currentUser = new AuthenticationState(newUser);
                NotifyAuthenticationStateChanged(Task.FromResult(currentUser));
            };

            var tokenCookie = _contextAccessor.HttpContext?.Request.Cookies
                .FirstOrDefault(c => c.Key == "authToken").Value;
            if (!string.IsNullOrEmpty(tokenCookie))
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"] ?? "your-default-key-here"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var issuer = _configuration["JwtSettings:Issuer"];
                var audience = _configuration["JwtSettings:Audience"];

                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    var claimsPrincipal = tokenHandler.ValidateToken(tokenCookie, validationParameters, out var validatedToken);
                    currentUser = new AuthenticationState(claimsPrincipal);
                }
                catch
                {
                    currentUser = new AuthenticationState(new ClaimsPrincipal());
                }
            }
            else
            {
                currentUser = new AuthenticationState(new ClaimsPrincipal());
            }
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
            Task.FromResult(currentUser);
    }
}
