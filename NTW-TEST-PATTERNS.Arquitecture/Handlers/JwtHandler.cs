using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Arquitecture.Handlers
{
    public interface IJwtHandler
    {
        string GenerateToken(string userId, List<string> roles = null, Dictionary<string, string> additionalClaims = null);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }

    public class JwtHandler : IJwtHandler
    {
        private readonly string _secret;
        private readonly int _tokenExpiration;
        private readonly string _claimUserIdName;
        private readonly string _claimRoleName;

        // Constructor to initialize required values, with optional claim name configuration
        public JwtHandler(string secret, int tokenExpiration, string claimUserIdName = "sub", string claimRoleName = ClaimTypes.Role)
        {
            _secret = secret ?? throw new ArgumentNullException(nameof(secret), "JWT Secret cannot be null");
            _tokenExpiration = tokenExpiration > 0 ? tokenExpiration : 30;
            _claimUserIdName = claimUserIdName;
            _claimRoleName = claimRoleName;
        }

        // Method to generate the token, with additional claims flexibility
        public string GenerateToken(string userId, List<string> roles = null, Dictionary<string, string> additionalClaims = null)
        {
            // Create the security key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create a list of claims
            var claims = new List<Claim>
            {
                // Add userId as the main claim with the configured claim name
                new Claim(_claimUserIdName, userId)
            };

            // Add additional claims if provided
            if (additionalClaims != null)
            {
                foreach (var claim in additionalClaims)
                {
                    claims.Add(new Claim(claim.Key, claim.Value));
                }
            }

            // If roles are provided, include each one in the claims
            if (roles != null && roles.Count > 0)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(_claimRoleName, role));
                }
            }

            // Set up the token description
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_tokenExpiration),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // Return the token as a string
            return tokenHandler.WriteToken(token);
        }

        // Method to validate the token and return the claims
        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            // Create the token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret))
            };

            // Create the token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Validate the token and return the claims
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
