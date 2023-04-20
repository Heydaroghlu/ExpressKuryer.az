using ExpressKuryer.Application.Abstractions.Token;
using ExpressKuryer.Application.DTOs.Token;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;
        readonly UserManager<AppUser> _userManager;
        public TokenHandler(IConfiguration configuration,UserManager<AppUser> userManager)
        {
           _configuration = configuration;
            _userManager = userManager;
        }
        public  TokenDTO CreateAccessToken(AppUser user,int minute)
        {
           TokenDTO token = new TokenDTO();
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            token.Expiration = DateTime.UtcNow.AddMinutes(minute);
            List<Claim> claims = CreateClaims(user);
            JwtSecurityToken securityToken = new(
                claims:claims,
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow,
                signingCredentials:signingCredentials
                );
          JwtSecurityTokenHandler tokenHandler = new();
           token.AccessToken= tokenHandler.WriteToken(securityToken);

            return token;
        }
        public  List<Claim> CreateClaims(AppUser user)
        {
            List<Claim> claims = new List<Claim>()
           {
               new Claim(ClaimTypes.NameIdentifier, user.Id),
               new Claim(ClaimTypes.Name,user.UserName),
               new Claim("Name",user.Name),
               new Claim("Surname",user.Surname),
               new Claim("IsAdmin",user.IsAdmin.ToString())
           };
            var adminRoles = _userManager.GetRolesAsync(user).Result;
            var roleClaims = adminRoles.Select(x => new Claim(ClaimTypes.Role, x));
            claims.AddRange(roleClaims);

            return claims;
        }
    }
}
