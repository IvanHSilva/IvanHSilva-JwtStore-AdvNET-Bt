using JwtStore.Core;
using JwtStore.Infra.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtStore.Api.Extensions;

public static class BuilderExtension {

    public static void AddConfiguration(this WebApplicationBuilder builder){
        Configuration.DatabaseConfiguration.ConnectionString =
                builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

        Configuration.Secrets.ApiKey =
            builder.Configuration.GetSection("Secrets").GetValue<string>("ApiKey") ?? string.Empty;
        Configuration.Secrets.JwtPrivateKey =
            builder.Configuration.GetSection("Secrets").GetValue<string>("JwtPrivateKey") ?? string.Empty;
        Configuration.Secrets.PasswordSaltKey =
            builder.Configuration.GetSection("Secrets").GetValue<string>("PasswordSaltKey") ?? string.Empty;
    }

    public static void AddDatabase(this WebApplicationBuilder builder) {
        builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(
            Configuration.DatabaseConfiguration.ConnectionString,
            b => b.MigrationsAssembly("JwtStore.Api")));
    }

    public static void AddJwtAuthentication(this WebApplicationBuilder builder) {
        builder.Services
            .AddAuthentication(a => {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(a => {
                a.RequireHttpsMetadata = false;
                a.SaveToken = true;
                a.TokenValidationParameters = new TokenValidationParameters {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.Secrets.JwtPrivateKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        builder.Services.AddAuthorization();
    }

}
