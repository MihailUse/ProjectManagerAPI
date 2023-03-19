using Application.DTO.Common;
using Application.DTO.MemberShip;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("Api/Project/{projectId:guid}/MemberShip")]
public class MemberShipController : ControllerBase
{
    private readonly IMemberShipService _memberShipService;

    public MemberShipController(IMemberShipService memberShipService)
    {
        _memberShipService = memberShipService;
    }

    [HttpGet]
    [CheckPermission(Role.MemberShip)]
    public async Task<PaginatedList<MemberShipDto>> Get(Guid projectId, [FromQuery] SearchMemberShipDto query)
    {
        return await _memberShipService.GetList(projectId, query);
    }

    [HttpPost]
    [CheckPermission(Role.Administrator)]
    public async Task<Guid> Post(Guid projectId, [FromBody] CreateMemberShipDto query)
    {
        return await _memberShipService.Create(projectId, query);
    }

    [HttpPut("{id:guid}")]
    [CheckPermission(Role.Administrator)]
    public async Task Put(Guid id, [FromBody] UpdateMemberShipDto query)
    {
        await _memberShipService.Update(id, query);
    }

    [HttpDelete("{id:guid}")]
    [CheckPermission(Role.Administrator)]
    public async Task Delete(Guid id)
    {
        await _memberShipService.Delete(id);
    }
}