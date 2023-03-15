using Application.DTO.Comment;
using Application.DTO.Common;

namespace Application.Interfaces.Services;

public interface ICommentService
{
    Task<PaginatedList<CommentDto>> GetList(SearchCommentDto searchDto);
    Task<Guid> Create(CreateCommentDto createDto);
    Task Update(Guid id, UpdateCommentDto updateDto);
}