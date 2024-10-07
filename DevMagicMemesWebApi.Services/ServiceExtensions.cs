using DevMagicMemesWebApi.Contracts;
using DevMagicMemesWebApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DevMagicMemesWebApi.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDevMagicMemesWebApiServices(this IServiceCollection services)
        {
            services.AddScoped<IMemeService, MemeService>();
            services.AddScoped<ITagService, TagService>();
            services.AddTransient<IFileService, FileService>();

            return services;
        }
    }
}
