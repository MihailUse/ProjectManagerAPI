using Application.DTO.Comment;
using Application.DTO.Common;

namespace Application.Interfaces.Services;

public interface ICommentService
{
    Task<PaginatedList<CommentDto>> GetList(Guid taskId, SearchCommentDto searchDto);
    Task<Guid> Create(Guid taskId, CreateCommentDto createDto);
    Task Update(Guid id, UpdateCommentDto updateDto);
}