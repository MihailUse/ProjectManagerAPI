using Application.DTO.Comment;
using Application.DTO.Common;
using Application.Interfaces.Services;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Filters;

namespace WebAPI.Controllers;

[CheckPermission(Role.MemberShip)]
[Authorize]
[ApiController]
[Route("Api/Project/{projectId:guid}/Task/{taskId:guid}/Comment")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<PaginatedList<CommentDto>> Get(Guid taskId, [FromQuery] SearchCommentDto query)
    {
        return await _commentService.GetList(taskId, query);
    }

    [HttpPost]
    public async Task<Guid> Post(Guid taskId, [FromBody] CreateCommentDto query)
    {
        return await _commentService.Create(taskId, query);
    }

    [HttpPut("{id:guid}")]
    public async Task Put(Guid id, [FromBody] UpdateCommentDto query)
    {
        await _commentService.Update(id, query);
    }
}