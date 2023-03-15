using Application.DTO.Comment;
using Application.DTO.MemberShip;
using Application.DTO.Project;
using Application.DTO.Status;
using Application.DTO.Task;
using Application.DTO.Team;
using Application.DTO.User;
using AutoMapper;
using Domain.Entities;
using Task = Domain.Entities.Task;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Projections

        // User
        CreateProjection<User, UserBriefDto>();

        // Project
        CreateProjection<Project, ProjectBriefDto>();
        CreateProjection<Project, ProjectDto>()
            .ForMember(d => d.TaskCount, m => m.MapFrom(s => s.Tasks.Count))
            .ForMember(d => d.MembershipCount, m => m.MapFrom(s => s.Memberships.Count));

        // MemberShip
        CreateProjection<MemberShip, MemberShipDto>();

        // Task
        CreateProjection<Task, TaskBriefDto>()
            .ForMember(d => d.Status, m => m.MapFrom(s => s.Status.Name))
            .ForMember(d => d.Assignees, m => m.MapFrom(s => s.Assignees.Select(x => x.MemberShip.User)));
        CreateProjection<Task, TaskDto>()
            .ForMember(d => d.Status, m => m.MapFrom(s => s.Status.Name))
            .ForMember(d => d.Assignees, m => m.MapFrom(s => s.Assignees.Select(x => x.MemberShip.User)));

        // Status
        CreateProjection<Status, StatusDto>();

        // Comment
        CreateProjection<Comment, CommentDto>();

        #endregion

        #region Maps

        // User
        CreateMap<User, UserDto>();
        CreateMap<UpdateUserDto, User>();

        // Project
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();
        CreateMap<Project, ProjectDto>();

        // MemberShip
        CreateMap<CreateMemberShipDto, MemberShip>();
        CreateMap<UpdateMemberShipDto, MemberShip>();
        CreateMap<MemberShip, MemberShipDto>();

        // Task
        CreateMap<CreateTaskDto, Task>();
        CreateMap<UpdateTaskDto, Task>();

        // Comment
        CreateMap<CreateCommentDto, Comment>();
        CreateMap<UpdateCommentDto, Comment>();

        // Status
        CreateMap<CreateStatusDto, Status>();
        CreateMap<UpdateStatusDto, Status>();
        CreateMap<Status, StatusDto>();

        // Team
        CreateMap<Team, TeamDto>();
        CreateMap<Team, TeamBriefDto>();
        CreateMap<CreateTeamDto, Team>();
        CreateMap<UpdateTeamDto, Team>();

        #endregion
    }
}