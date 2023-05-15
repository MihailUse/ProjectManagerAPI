using Application.DTO.Comment;
using Application.DTO.Common;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

public class CommentService : ICommentService
{
    private readonly IMapper _mapper;
    private readonly ICommentRepository _repository;
    private readonly ITaskService _taskService;
    private readonly Guid _currentUserId;

    public CommentService(
        IMapper mapper,
        ICommentRepository repository,
        ITaskService taskService,
        ICurrentUserService currentUserService
    )
    {
        _mapper = mapper;
        _repository = repository;
        _taskService = taskService;
        _currentUserId = currentUserService.UserId;
    }

    public async Task<PaginatedList<CommentDto>> GetList(Guid taskId, SearchCommentDto searchDto)
    {
        return await _repository.GetList(taskId, searchDto);
    }

    public async Task<Guid> Create(Guid taskId, CreateCommentDto createDto)
    {
        var task = await _taskService.FindTask(taskId);

        var comment = _mapper.Map<Comment>(createDto);
        comment.OwnerId = _currentUserId;
        comment.Task = task;

        await _repository.Add(comment);
        return comment.Id;
    }

    public async Task Update(Guid id, UpdateCommentDto updateDto)
    {
        var comment = await _repository.FindById(id);
        if (comment == default)
            throw new NotFoundException("Comment not found");

        if (comment.OwnerId != _currentUserId)
            throw new AccessDeniedException("No permission");

        comment = _mapper.Map(updateDto, comment);
        await _repository.Update(comment);
    }
}