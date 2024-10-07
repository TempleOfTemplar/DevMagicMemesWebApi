using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DevMagicMemesWebApi.Controllers
{
    internal sealed class EntityValidatorProvider : IModelValidatorProvider
    {
        private readonly IActionContextAccessor _actionContextAccessor;

        public EntityValidatorProvider(IActionContextAccessor actionContextAccessor)
        {
            _actionContextAccessor = actionContextAccessor
                ?? throw new ArgumentNullException(nameof(actionContextAccessor));
        }

        public void CreateValidators(ModelValidatorProviderContext context)
        {
            if (context.ValidatorMetadata.Count > 0 &&
                _actionContextAccessor.ActionContext is { ActionDescriptor: ControllerActionDescriptor descriptor })
            {
                if (descriptor.MethodInfo.GetCustomAttribute<EntityValidationAttribute>() is not null ||
                    descriptor.MethodInfo.DeclaringType?.GetCustomAttribute<EntityValidationAttribute>() is not null)
                {
                    if (context.ModelMetadata.MetadataKind is ModelMetadataKind.Property &&
                        context.ModelMetadata.PropertyName is string propertyName &&
                        context.ModelMetadata.ContainerType is Type containerType &&
                        containerType.GetProperty(propertyName) is PropertyInfo property)
                    {
                        var required = context.Results
                            .FirstOrDefault(x => x.ValidatorMetadata is RequiredAttribute);

                        if (required is not null &&
                            property.GetCustomAttribute<RequiredAttribute>() is null &&
                            context.ModelMetadata.IsComplexType &&
                            !context.ModelMetadata.IsEnumerableType)
                        {
                            required.Validator = null;
                        }
                    }
                }
            }
        }
    }
}
