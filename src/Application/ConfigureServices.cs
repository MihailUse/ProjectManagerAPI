using Application.Interfaces.Services;
using Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Configs;
using Microsoft.Extensions.Configuration;

namespace Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Configure
        var imageGeneratorSection = configuration.GetSection(ImageGeneratorConfig.Position);
        if (imageGeneratorSection == null)
            throw new InvalidOperationException("ImageGeneratorConfig not found.");

        services.Configure<ImageGeneratorConfig>(imageGeneratorSection);

        // services
        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IStatusService, StatusService>();
        services.AddScoped<IMemberShipService, MemberShipService>();
        services.AddScoped<IAttachService, AttachService>();
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<ICommentService, CommentService>();

        return services;
    }
}