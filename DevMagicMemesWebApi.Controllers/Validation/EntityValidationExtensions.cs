using DevMagicMemesWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EntityValidationExtensions
    {
        public static IServiceCollection AddModelValidator(this IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddOptions<MvcOptions>().Configure<IActionContextAccessor>((options, accessor)
                => options.ModelValidatorProviders.Add(new EntityValidatorProvider(accessor)));

            return services;
        }
    }
}
