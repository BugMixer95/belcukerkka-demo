using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;

namespace WebApplication.Infrastructure
{
    public class DoubleModelBinderProvider : IModelBinderProvider
    {
        private IModelBinder binder;

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            ILoggerFactory logger = context.Services.GetService(typeof(ILoggerFactory)) as ILoggerFactory;

            if (context.Metadata.ModelType == typeof(double))
                binder = new DoubleModelBinder(new SimpleTypeModelBinder(typeof(double), logger));

            else if (context.Metadata.ModelType == typeof(float))
                binder = new DoubleModelBinder(new SimpleTypeModelBinder(typeof(float), logger));

            else if (context.Metadata.ModelType == typeof(decimal))
                binder = new DoubleModelBinder(new SimpleTypeModelBinder(typeof(decimal), logger));

            else
                return null;

            return binder;
        }
    }
}
