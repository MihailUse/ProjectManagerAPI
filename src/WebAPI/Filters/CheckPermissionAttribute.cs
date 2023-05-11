using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters;

public class CheckPermissionAttribute : ActionFilterAttribute
{
    private readonly Role _role;

    public CheckPermissionAttribute(Role role)
    {
        _role = role;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var projectService = context.HttpContext.RequestServices.GetService<IProjectService>();
        var projectIdArgument = context.ActionArguments.FirstOrDefault(x => x.Key == "projectId").Value;

        var isValidGuid = Guid.TryParse(projectIdArgument?.ToString(), out var projectId);
        if (!isValidGuid)
        {
            context.Result = new NotFoundResult();
            return;
        }

        await projectService!.CheckPermission(projectId, _role);
        await next();
    }
}