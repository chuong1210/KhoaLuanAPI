
using Domain.Services.Repositories;
using Infrastructure;
using Infrastructure.Interceptors;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sieve.Services;
using System;
using System.Reflection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
                this IServiceCollection services,
                IConfiguration configuration)
    {

        services.AddScoped<EntitySaveChangesInterceptor>();

        var connectionString = configuration.GetConnectionString("DefaultConnection"); // Get from appsettings.json

        services.AddDbContext<ShopDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null);
                    npgsqlOptions.CommandTimeout(30);
                    npgsqlOptions.MigrationsAssembly(typeof(ShopDbContext).Assembly.FullName);
                });
        });

        services.AddScoped<IBannerRepository, BannerRepository>();

        services.AddScoped<IShopRepository, ShopRepository>();
        services.AddScoped<IProgressTransferRepository, ProgressTransferRepository>();
        services.AddScoped<IOfflineTransactionRepository, OfflineTransactionRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IVoucherShopRepository, VoucherShopRepository>();

  

        return services;
    }
}