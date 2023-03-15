using Application.DTO.Auth;
using Application.DTO.Common;
using Application.DTO.User;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Services;

internal class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _repository;
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAttachService _attachService;
    private readonly IHashGenerator _hashGenerator;

    public UserService(
        IMapper mapper,
        IUserRepository repository,
        IIdentityService identityService,
        ICurrentUserService currentUserService,
        IAttachService attachService,
        IHashGenerator hashGenerator
    )
    {
        _mapper = mapper;
        _repository = repository;
        _identityService = identityService;
        _currentUserService = currentUserService;
        _attachService = attachService;
        _hashGenerator = hashGenerator;
    }

    public async Task<UserDto> Get(Guid id)
    {
        var user = await GetById(id);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<PaginatedList<UserBriefDto>> GetList(SearchUserDto searchDto)
    {
        return await _repository.GetList(searchDto);
    }

    public async Task<AccessTokensDto> Create(CreateUserDto createDto)
    {
        var loginExists = await _repository.CheckLoginExists(createDto.Login);
        if (loginExists)
            throw new ConflictException("Login already exists");

        // create user
        var userSession = new UserSession();
        var user = new User()
        {
            Login = createDto.Login,
            AvatarId = await _attachService.GenerateImage(),
            PasswordHash = _hashGenerator.GetHash(createDto.Password),
            UserSessions = new List<UserSession>()
            {
                userSession
            }
        };

        await _repository.Add(user);
        return _identityService.GenerateTokens(userSession);
    }

    public async Task Update(Guid id, UpdateUserDto updateDto)
    {
        var user = await GetById(id);

        if (user.AvatarId != updateDto.AvatarId)
            await _attachService.CheckAttachExists(updateDto.AvatarId);

        user = _mapper.Map(updateDto, user);
        await _repository.Update(user);
    }

    public async Task Delete(Guid id)
    {
        if (_currentUserService.UserId != id)
            throw new AccessDeniedException("No permission");

        var user = await GetById(id);
        await _repository.Remove(user);
    }

    public async Task CheckUserExists(Guid userId)
    {
        var userExists = await _repository.CheckExists(userId);
        if (!userExists)
            throw new NotFoundException("User not found");
    }

    private async Task<User> GetById(Guid id)
    {
        var user = await _repository.FindById(id);
        if (user == default)
            throw new NotFoundException("User not found");

        return user;
    }
}