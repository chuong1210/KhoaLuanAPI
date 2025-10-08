

using Application.Cache;
using Application.Cache.Interfaces;
using Application.IntegrationEvents.HttpClients;
using Application.IntegrationEvents.HttpClients.Dtos;
using Application.IntegrationEvents.HttpClients.Interfaces;
using Application.IntegrationEvents.Incoming;
using Application.Interfaces.Identity;
using Application.Interfaces.MessageBroker;
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure;
using Infrastructure.Configurations ;
using Infrastructure.IntegrationEvents;
using Infrastructure.IntegrationEvents.MessageBroker;
using Infrastructure.IntegrationEvents.MessageBroker.EventHandlers;
using Infrastructure.IntegrationEvents.MessageBroker.Kafka;
using Infrastructure.IntegrationEvents.MessageBroker.Kafka.Interfaces;
using Infrastructure.Interceptors;
using Infrastructure.Repositories;
using KhoaLuanTotNghiepAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using System.Text;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

// ============ SERVICES CONFIGURATION ============

// Controllers with JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// API Explorer & Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Shop Service API",
        Version = "v1",
        Description = "Microservices API for Shop, Banner, Cart, Transfer management"
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Current User Service
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// PostgreSQL Database
builder.Services.AddDbContext<ShopDbContext>((serviceProvider,options) =>
{
    var interceptor = serviceProvider.GetRequiredService<EntitySaveChangesInterceptor>();
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorCodesToAdd: null);
            npgsqlOptions.CommandTimeout(30);
            npgsqlOptions.MigrationsAssembly(typeof(ShopDbContext).Assembly.FullName);
        });

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }

    options.AddInterceptors(interceptor);
});


//if (builder.Environment.IsDevelopment())
//{
//    builder.Services.AddDbContext<ShopDbContext>(options =>
//    {
//        options.EnableSensitiveDataLogging();
//        options.EnableDetailedErrors();
//    });
//}
// Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "ShopService_";
});

// Cache Service
builder.Services.AddSingleton<ICacheService, RedisCacheService>();

// HTTP Clients with Polly (Circuit Breaker + Retry)
builder.Services.AddHttpClient<IProductServiceClient, ProductServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductService"]);
    client.Timeout = TimeSpan.FromSeconds(10);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.AddPolicyHandler(GetRetryPolicy())
.AddPolicyHandler(GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IProfileServiceClient, ProfileServiceClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProfileService"]);
    client.Timeout = TimeSpan.FromSeconds(10);
})
.AddPolicyHandler(GetRetryPolicy())
.AddPolicyHandler(GetCircuitBreakerPolicy());

// Kafka Event Bus
builder.Services.AddSingleton<IEventBus, KafkaEventBus>();
//builder.Services.AddHostedService<KafkaConsumerHostedService>();

// Event Handlers
builder.Services.AddScoped<IEventHandler<ProductUpdatedEvent>, ProductEventHandler>();
builder.Services.AddScoped<IEventHandler<ProductDeletedEvent>, ProductEventHandler>();
builder.Services.AddScoped<IEventHandler<OrderShopCompletedEvent>, OrderEventHandler>();
builder.Services.AddScoped<IEventHandler<OrderShopCancelledEvent>, OrderEventHandler>();
builder.Services.AddScoped<IEventHandler<AddressUpdatedEvent>, AddressEventHandler>();


builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices (builder.Configuration);


// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

// Authorization
builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

//// Health Checks
//builder.Services.AddHealthChecks()
//    .AddNpgSql(
//        builder.Configuration.GetConnectionString("PostgreSQL"),
//        name: "postgresql",
//        tags: new[] { "db", "postgresql" })
//    .AddRedis(
//        builder.Configuration.GetConnectionString("Redis"),
//        name: "redis",
//        tags: new[] { "cache", "redis" })
//    .AddUrlGroup(
//        new Uri(builder.Configuration["ServiceUrls:ProductService"] + "/health"),
//        name: "product-service",
//        tags: new[] { "services" })
//    .AddUrlGroup(
//        new Uri(builder.Configuration["ServiceUrls:ProfileService"] + "/health"),
//        name: "profile-service",
//        tags: new[] { "services" });
// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();
// Always enable Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop Service API V1");
    //c.RoutePrefix = string.Empty; // Swagger UI tại URL gốc "/"
});
// ============ MIDDLEWARE PIPELINE ============

// Global Exception Handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var error = context.Features.Get<IExceptionHandlerFeature>();
        if (error != null)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(error.Error, "Unhandled exception occurred");

            await context.Response.WriteAsJsonAsync(new
            {
                succeeded = false,
                code = 500,
                messages = new[] { "An internal server error occurred" }
            });
        }
    });
});

// Swagger
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shop Service API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//app.MapHealthChecks("/health");

// Database Migration (in Development only)
//if (app.Environment.IsDevelopment())
//{
//    using var scope = app.Services.CreateScope();
//    var dbContext = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
//    await dbContext.Database.MigrateAsync();
//}

app.Run();

// Polly Policies
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (outcome, timespan, retryCount, context) =>
            {
                Console.WriteLine($"Retry {retryCount} after {timespan.TotalSeconds}s delay");
            });
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30),
            onBreak: (outcome, timespan) =>
            {
                Console.WriteLine($"Circuit breaker opened for {timespan.TotalSeconds}s");
            },
            onReset: () =>
            {
                Console.WriteLine("Circuit breaker reset");
            });
}
