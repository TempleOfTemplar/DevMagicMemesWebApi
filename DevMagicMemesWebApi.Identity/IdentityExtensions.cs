using DevMagicMemesWebApi.Common;
using DevMagicMemesWebApi.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;

namespace DevMagicMemesWebApi.Identity
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityAuthentication(
            this IServiceCollection services,
            Action<JwtTokenOptions> jwtOptionsAction,
            Action<DbContextOptionsBuilder> dbContextOptionsAction)
        {
            services.AddIdentityCore<IdentityUser>()
                .AddSignInManager()
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<IdentityDataContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<ICurrentUser, IdentityCurrentUser>();

            var jwtOptions = new JwtTokenOptions();

            jwtOptionsAction.Invoke(jwtOptions);

            services.Configure(jwtOptionsAction);

            services.AddDbContext<IdentityDataContext>(dbContextOptionsAction);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var symmetricKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtOptions.SigningKey));

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateIssuer = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateAudience = true,
                        IssuerSigningKey = symmetricKey,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                    };

                    options.IncludeErrorDetails = true;

                    IdentityModelEventSource.ShowPII = true;
                });

            return services;
        }

        public static IServiceProvider UseIdentityDatabaseMigrate(this IServiceProvider provider)
        {
            using var serviceScope = provider.CreateScope();

            var serviceProvider = serviceScope.ServiceProvider;

            var context = serviceProvider.GetRequiredService<IdentityDataContext>();

            context.Database.Migrate();

            return provider;
        }
    }
}
