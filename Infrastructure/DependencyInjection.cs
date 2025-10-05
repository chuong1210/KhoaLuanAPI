
using Infrastructure;
using Infrastructure.Repositories;
using Domain.Services.Repositories;
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
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
  
    //    services.AddDbContext<ShopDbContext>(options =>
    //options.UseNpgsql(
    //    configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IBannerRepository, BannerRepository>();

        services.AddScoped<IShopRepository, ShopRepository>();
        services.AddScoped<IProgressTransferRepository, ProgressTransferRepository>();
        services.AddScoped<IOfflineTransactionRepository, OfflineTransactionRepository>();
        services.AddScoped<ICartRepository, CartRepository>();

        return services;
    }
}