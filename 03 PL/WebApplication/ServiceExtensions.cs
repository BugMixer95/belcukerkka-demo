using Microsoft.Extensions.DependencyInjection;
using Belcukerkka.Models.Entities;
using Belcukerkka.Repositories.Interfaces;
using Belcukerkka.Repositories.Repos;
using Microsoft.AspNetCore.Builder;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;

namespace WebApplication
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Registers repositories and their interface dependencies.
        /// </summary>
        public static IServiceCollection RegisterRepositoryService(this IServiceCollection services)
        {
            return services
                .AddScoped<IEntityRepository<Box>, SqlBoxRepository>()
                .AddScoped<IEntityRepository<BoxParent>, SqlBoxParentRepository>()
                .AddScoped<IEntityRepository<BoxPackage>, SqlBoxPackageRepository>()
                .AddScoped<IEntityRepository<Candy>, SqlCandyRepository>()
                .AddScoped<IEntityRepository<Composition>, SqlCompositionRepository>()
                .AddScoped<IEntityRepository<Customer>, SqlCustomerRepository>()
                .AddScoped<IEntityRepository<Order>, SqlOrderRepository>()
                .AddScoped<IEntityRepository<OrderItem>, SqlOrderItemRepository>()
                .AddScoped<IEntityRepository<WeightType>, SqlWeightTypeRepository>()
                .AddScoped<ICatalogItemRepository, SqlCatalogItemRepository>()
                .AddScoped<IEntityRepository<User>, SqlUserRepository>();
        }

        /// <summary>
        /// Registers Russian culture info as a default one (along with a dot decimal separator).
        /// </summary>
        public static IServiceCollection RegisterCultureInfo(this IServiceCollection services)
        {
            CultureInfo[] supportedCultures = new[]
            {
                new CultureInfo("en-GB"),
                new CultureInfo("ru-RU")
            };

            CultureInfo defaultCulture = new CultureInfo("ru-RU");
            defaultCulture.NumberFormat.NumberDecimalSeparator = ".";
            defaultCulture.NumberFormat.CurrencyDecimalSeparator = ".";

            return services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>()
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });
        }
    }
}
