using Application.DTO.Comment;
using Application.DTO.Common;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Services;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class CommentService : ICommentService
{
    private readonly IMapper _mapper;
    private readonly IDatabaseContext _database;
    private readonly Guid _currentUserId;

    public CommentService(IMapper mapper, IDatabaseContext database, ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _database = database;
        _currentUserId = currentUserService.UserId;
    }

    public async Task<PaginatedList<CommentDto>> GetList(SearchCommentDto searchDto)
    {
        await CheckPermission(searchDto.TaskId, Role.MemberShip);

        return await _database.Comments
            .Where(x => x.TaskId == searchDto.TaskId)
            .ProjectToPaginatedListAsync<CommentDto>(_mapper.ConfigurationProvider, searchDto);
    }

    public async Task<Guid> Create(CreateCommentDto createDto)
    {
        await CheckPermission(createDto.TaskId, Role.MemberShip);

        var comment = _mapper.Map<Comment>(createDto);
        comment.OwnerId = _currentUserId;

        await _database.Comments.AddAsync(comment);
        await _database.SaveChangesAsync();

        return comment.Id;
    }

    public async Task Update(Guid id, UpdateCommentDto updateDto)
    {
        var comment = await _database.Comments.FirstOrDefaultAsync(x => x.Id == id);
        if (comment == default)
            throw new NotFoundException("Comment not found");

        if (comment.OwnerId != _currentUserId)
            throw new AccessDeniedException("No permission");

        comment = _mapper.Map(updateDto, comment);
        _database.Comments.Update(comment);
        await _database.SaveChangesAsync();
    }

    private async Task CheckPermission(Guid taskId, Role role)
    {
        var task = await _database.Tasks
            .Include(x => x.Project.Memberships.Where(m => m.UserId == _currentUserId))
            .FirstOrDefaultAsync(x => x.Id == taskId);
        if (task == default)
            throw new NotFoundException("Task not found");

        var currentMemberShip = task.Project.Memberships.FirstOrDefault();
        if (currentMemberShip == default || currentMemberShip.Role > role)
            throw new AccessDeniedException("No permission");
    }
}