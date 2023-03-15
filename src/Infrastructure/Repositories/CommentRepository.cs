using Application.DTO.Comment;
using Application.DTO.Common;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly IMapper _mapper;
    private readonly DatabaseContext _database;

    public CommentRepository(IMapper mapper, DatabaseContext database)
    {
        _mapper = mapper;
        _database = database;
    }

    public async Task<Comment?> FindById(Guid id)
    {
        return await _database.Comments.FindAsync(id);
    }

    public async Task<PaginatedList<CommentDto>> GetList(Guid taskId, SearchCommentDto searchDto)
    {
        return await _database.Comments
            .Where(x => x.TaskId == taskId && EF.Functions.ILike(x.Text, $"%{searchDto.Search}%"))
            .ProjectToPaginatedList<Comment, CommentDto>(_mapper.ConfigurationProvider, searchDto);
    }

    public async Task Add(Comment comment)
    {
        await _database.Comments.AddAsync(comment);
        await _database.SaveChangesAsync();
    }

    public async Task Update(Comment comment)
    {
        _database.Comments.Update(comment);
        await _database.SaveChangesAsync();
    }
}