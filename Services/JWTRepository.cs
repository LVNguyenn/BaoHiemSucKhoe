using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InsuranceManagement.DTOs;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace InsuranceManagement.Services
{
    public interface ITokenRepository
    {
        string CreateJWTToken(UserDTO user);
    }

    public class JWTRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public JWTRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateJWTToken(UserDTO user)
        {
            // Create claim
            var claim = new List<Claim>();
            claim.Add(new Claim("displayName", user.displayName));
            claim.Add(new Claim("email", user.email));
            claim.Add(new Claim("phone", user.phone));
            claim.Add(new Claim("image", user.image != null ? user.image : ""));
            //claim.Add(new Claim("role", user.role));

            // create key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            // create credentials 
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            // create token

            var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claim,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
