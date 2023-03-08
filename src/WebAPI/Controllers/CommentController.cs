using Application.DTO.Comment;
using Application.DTO.Common;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class CommentController
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet]
    public async Task<PaginatedList<CommentDto>> Get([FromQuery] SearchCommentDto query)
    {
        return await _commentService.GetList(query);
    }

    [HttpPost]
    public async Task<Guid> Post([FromBody] CreateCommentDto query)
    {
        return await _commentService.Create(query);
    }

    [HttpPut("{id:guid}")]
    public async Task Put(Guid id, [FromBody] UpdateCommentDto query)
    {
        await _commentService.Update(id, query);
    }
}