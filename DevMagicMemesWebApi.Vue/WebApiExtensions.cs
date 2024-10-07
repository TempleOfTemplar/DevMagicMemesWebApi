using DevMagicMemesWebApi.Dal;
using DevMagicMemesWebApi.Identity;
using DevMagicMemesWebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Text.Json.Serialization;

namespace Microsoft.AspNetCore.Builder
{
    public static class WebApiExtensions
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString().Replace('+', '.'));
                options.MapType<DateOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "date"
                });
                options.MapType<TimeOnly>(() => new OpenApiSchema
                {
                    Type = "string",
                    Format = "time",
                    Example = new OpenApiString(DateTime.Now.ToLongTimeString())
                });

                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                options.OperationFilter<SecurityRequirementsOperationFilter>();
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Please enter the Bearer token",
                    Name = "Authorization",
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http
                });
            });

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DataContext"));
            });

            builder.Services.AddRepository();
            builder.Services.AddDevMagicMemesWebApiServices();
            builder.Services.AddModelValidator();
            builder.Services.AddHttpClient();

            builder.Services.AddIdentityAuthentication(
                jwtOptions => builder.Configuration.Bind("Authentication:JwtBearer", jwtOptions),
                dbContextOptions => dbContextOptions.UseNpgsql(builder.Configuration.GetConnectionString("IdentityDataContext")));


            return builder;
        }

        public static WebApplication Configure(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(app.Environment.ContentRootPath, "Uploads")),
                    RequestPath = "/Resources"
                });

                app.UseCors(options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.AllowAnyOrigin();
                });

                app.Services.UseDatabaseEnsureCreated<DataContext>();
                app.Services.UseIdentityDatabaseMigrate();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers().RequireAuthorization();

            return app;
        }
    }
}
