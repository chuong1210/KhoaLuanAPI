using Application.Cache.Interfaces;
using Application.Interfaces.Services;
using Application.Profiles;
using Application.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sieve.Services;
using System;
using System.Reflection;
using Application.Cache;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddSingleton(static provider => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new CommonMappingProfile());
            cfg.AddProfile(new ModuleMappingProfile());
        }, provider.GetService<ILoggerFactory>()).CreateMapper());
        //var config = new MapperConfiguration(cfg =>
        //{
        //    cfg.CreateMap<Source, Destination>();
        //}, new NullLoggerFactor


        services.AddScoped<ISieveProcessor, SieveProcessor>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IShopService, ShopService>();
        services.AddScoped<IBannerService, BannerService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<ICacheService, RedisCacheService>();

        services.AddScoped<ITransferService, TransferService>();

        return services;
    }
}