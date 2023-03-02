using Application.DTO.Common;
using Application.DTO.Status;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class StatusController : ControllerBase
{
    private readonly IStatusService _statusService;

    public StatusController(IStatusService statusService)
    {
        _statusService = statusService;
    }

    [HttpGet]
    public async Task<PaginatedList<StatusDto>> Get([FromQuery] SearchStatusDto query)
    {
        return await _statusService.GetList(query);
    }

    [HttpPost]
    public async Task<Guid> Post([FromBody] CreateStatusDto query)
    {
        return await _statusService.Create(query);
    }

    [HttpPut("{id:guid}")]
    public async Task Put(Guid id, [FromBody] UpdateStatusDto query)
    {
        await _statusService.Update(id, query);
    }

    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)
    {
        await _statusService.Delete(id);
    }
}