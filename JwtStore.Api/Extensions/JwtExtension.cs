using JwtStore.Core;
using JwtStore.Core.Contexts.AccountContext.UseCases.Authenticate;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtStore.Api.Extensions; 

public static class JwtExtension {

    public static string Generate(ResponseData data) {
        JwtSecurityTokenHandler handler = new();
        byte[] key = Encoding.ASCII.GetBytes(Configuration.Secrets.JwtPrivateKey);
        SigningCredentials credentials = new(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        SecurityTokenDescriptor tokenDescriptor = new() 
        {
            Subject = GenerateClaims(data),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = credentials,
        };
        SecurityToken token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(ResponseData user) {
        ClaimsIdentity ci = new();
        ci.AddClaim(new Claim("Id", user.Id));
        ci.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
        ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));
        foreach (var role in user.Roles)
            ci.AddClaim(new Claim(ClaimTypes.Role, role));

        return ci;
    }
}
