using Application.Common.Interfaces;
using Domain.Services;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<SaveChangesInterceptor, TimestampSaveChangesInterceptor>();
        services.AddSingleton<IImageGeneratorService, ImageGeneratorService>();
        services.AddSingleton<IHashGeneratorService, HashGeneratorService>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (connectionString == null)
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(connectionString, builder =>
                builder.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName)));

        services.AddScoped<IDatabaseContext>(x => x.GetRequiredService<DatabaseContext>());
        services.AddScoped<DatabaseInitialiser>();

        return services;
    }
}
