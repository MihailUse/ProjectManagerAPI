using Application.Interfaces;
using Application.Interfaces.Services;
using Domain.Services;
using Infrastructure.Configs;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var authSection = configuration.GetSection(AuthConfig.Position);
        if (authSection == null)
            throw new InvalidOperationException("AuthConfig not found.");

        var imageGeneratorSection = configuration.GetSection(AuthConfig.Position);
        if (imageGeneratorSection == null)
            throw new InvalidOperationException("ImageGeneratorConfig not found.");

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (connectionString == null)
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.Configure<AuthConfig>(authSection);
        services.Configure<ImageGeneratorConfig>(imageGeneratorSection);
        services.AddSingleton<SaveChangesInterceptor, TimestampSaveChangesInterceptor>();
        services.AddSingleton<IImageGeneratorService, ImageGeneratorService>();
        services.AddSingleton<IHashGeneratorService, HashGeneratorService>();
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(connectionString, builder =>
                builder.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName)));

        services.AddScoped<IDatabaseContext>(x => x.GetRequiredService<DatabaseContext>());
        services.AddScoped<DatabaseInitialiser>();

        return services;
    }
}
