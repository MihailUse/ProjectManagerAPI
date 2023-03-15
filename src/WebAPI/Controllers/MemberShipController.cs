using Application.DTO.Common;
using Application.DTO.MemberShip;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class MemberShipController : ControllerBase
{
    private readonly IMemberShipService _memberShipService;

    public MemberShipController(IMemberShipService memberShipService)
    {
        _memberShipService = memberShipService;
    }

    [HttpGet]
    public async Task<PaginatedList<MemberShipDto>> Get([FromQuery] SearchMemberShipDto query)
    {
        return await _memberShipService.GetList(query);
    }

    [HttpPost]
    public async Task<Guid> Post([FromBody] CreateMemberShipDto query)
    {
        return await _memberShipService.Create(query);
    }

    [HttpPut("{id:guid}")]
    public async Task Put(Guid id, [FromBody] UpdateMemberShipDto query)
    {
        await _memberShipService.Update(id, query);
    }

    [HttpDelete("{id:guid}")]
    public async Task Delete(Guid id)
    {
        await _memberShipService.Delete(id);
    }
}