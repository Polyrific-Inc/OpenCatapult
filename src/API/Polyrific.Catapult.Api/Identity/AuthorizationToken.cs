// Copyright (c) Polyrific, Inc 2018. All rights reserved.

using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;

namespace Polyrific.Catapult.Api.Identity
{
    public class AuthorizationToken
    {
        public static string GenerateToken(int userId, string userName, string firstName, string lastName, string userRole, 
            List<(int, string, string)> userProjects, string tokenKey, string tokenIssuer, string tokenAudience)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var token = new JwtSecurityToken(
                issuer: tokenIssuer,
                audience: tokenAudience,
                claims: new[]
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.GivenName, firstName ?? string.Empty),
                    new Claim(ClaimTypes.Surname, lastName ?? string.Empty),
                    new Claim(CustomClaimTypes.Projects, JsonConvert.SerializeObject(userProjects.Select(up => new ProjectClaim
                    {
                        ProjectId = up.Item1,
                        ProjectName = up.Item2,
                        MemberRole = up.Item3
                    }))), 
                    new Claim(ClaimTypes.Role, userRole)
                },
                expires: DateTime.UtcNow.AddHours(1),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateEngineToken(string engineName, string engineRole, string tokenKey, string tokenIssuer, string tokenAudience, DateTime? expires)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var token = new JwtSecurityToken(
                issuer: tokenIssuer,
                audience: tokenAudience,
                claims: new[]
                {
                    new Claim(ClaimTypes.Name, engineName), 
                    new Claim(ClaimTypes.Role, engineRole)
                },
                expires: expires,
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
