using Common;
using Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sservices
{
    public class JwtService : IJwtService
    {
        protected readonly SiteSettings _siteSetting;
        public JwtService(IOptionsSnapshot<SiteSettings> siteSetting)
        {
            this._siteSetting = siteSetting.Value;
        }
        public string Generate(User user)
        {
            var securityKey = Encoding.UTF8.GetBytes(this._siteSetting.JwtSettings.SecretKey); // longer than 16 charecters
            var signInCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature);
            // in order to make the key secure
            //var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.EncryptKey); //must be 16 character
            //var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = getClaims(user);

            var descripter = new SecurityTokenDescriptor
            {
                Issuer = this._siteSetting.JwtSettings.Issuer,
                Audience = this._siteSetting.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(this._siteSetting.JwtSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(this._siteSetting.JwtSettings.ExpirationMinutes),
                SigningCredentials = signInCredentials,
                //EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descripter);
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }

        private IEnumerable<Claim> getClaims(User user)
        {
            var claimsList = new List<Claim> {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier,user.ID.ToString())
            };
            var roles = new List<Role>();

            foreach (var role in user.UserRoles)
            {
                roles.Add(new Role { Name = role.Role.Name });
            }

            foreach (var role in roles)
            {
                claimsList.Add(new Claim(ClaimTypes.Role, role.Name));
            }
            return claimsList;
        }
    }
}
