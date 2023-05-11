using Application.DTO.Common;
using Application.DTO.Status;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("Api/Project/{projectId:guid}/Status")]
public class StatusController : ControllerBase
{
    private readonly IStatusService _statusService;

    public StatusController(IStatusService statusService)
    {
        _statusService = statusService;
    }

    [HttpGet]
    [CheckPermission(Role.MemberShip)]
    public async Task<PaginatedList<StatusDto>> Get([FromRoute] Guid projectId, [FromQuery] SearchStatusDto query)
    {
        return await _statusService.GetList(projectId, query);
    }

    [HttpPost]
    [CheckPermission(Role.Administrator)]
    public async Task<Guid> Post([FromRoute] Guid projectId, [FromBody] CreateStatusDto query)
    {
        return await _statusService.Create(projectId, query);
    }

    [HttpPut("{id:guid}")]
    [CheckPermission(Role.Administrator)]
    public async Task Put(Guid id, Guid projectId, [FromBody] UpdateStatusDto query)
    {
        await _statusService.Update(id, query);
    }

    [HttpDelete("{id:guid}")]
    [CheckPermission(Role.Administrator)]
    public async Task Delete(Guid id, Guid projectId)
    {
        await _statusService.Delete(id);
    }
}