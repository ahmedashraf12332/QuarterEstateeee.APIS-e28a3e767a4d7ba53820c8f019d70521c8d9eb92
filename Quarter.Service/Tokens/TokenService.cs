using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Quarter.Core.Entities.Identity;
using Quarter.Core.Services.Contract;
namespace Quarter.Service.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            var authClaims = new List<Claim>();

            // تأكد إن الـ Email مش فاضي
            if (!string.IsNullOrWhiteSpace(user.Email))
                authClaims.Add(new Claim(ClaimTypes.Email, user.Email));
            else
                throw new ArgumentNullException(nameof(user.Email), "Email is required to create a token");

            // DisplayName
            authClaims.Add(new Claim(ClaimTypes.GivenName, user.DisplayName ?? "Unknown"));

            // PhoneNumber
            authClaims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? "0000000000"));

            // الأدوار
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // إنشاء التوكن
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["Jwt:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
