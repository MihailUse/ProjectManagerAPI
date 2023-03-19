using Application.DTO.Comment;
using Application.DTO.Common;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface ICommentRepository
{
    Task<Comment?> FindById(Guid id);
    Task<PaginatedList<CommentDto>> GetList(Guid taskId, SearchCommentDto searchDto);
    Task Add(Comment comment);
    Task Update(Comment comment);
}