using Application.DTO.Comment;
using Application.DTO.MemberShip;
using Application.DTO.Project;
using Application.DTO.Status;
using Application.DTO.Task;
using Application.DTO.Team;
using Application.DTO.User;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Task = Domain.Entities.Task;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // parameters 
        Guid? currentUserId = null;

        #region Projections

        // User
        CreateProjection<User, UserBriefDto>();
        CreateProjection<User, UserDto>()
            .ForMember(
                d => d.CountCommonProjects,
                m => m.MapFrom(
                    s => s.MemberShips.Count(x => x.Project.Memberships.Any(ms => ms.UserId == currentUserId))
                )
            )
            .ForMember(
                d => d.CountAssignedTasks,
                m => m.MapFrom(
                    s => s.MemberShips
                        .Where(x => x.Project.Memberships.Any(ms => ms.UserId == currentUserId))
                        .Select(x => x.Project.Tasks)
                        .Count(
                            x => x.Any(t =>
                                t.Assignees.Any(a => a.MemberShip.UserId == currentUserId) ||
                                t.AssigneeTeams.Any(at => at.Team.MemberShips.Any(tm => tm.UserId == currentUserId))
                            )
                        )
                )
            )
            .ForMember(
                d => d.CountCreatedProjects,
                m => m.MapFrom(s => s.MemberShips.Count(x => x.Role == Role.Owner))
            )
            .ForMember(
                d => d.CountCreatedTasks,
                m => m.MapFrom(s => s.Tasks.Count)
            )
            .ForMember(
                d => d.CountWrittenComments,
                m => m.MapFrom(s => s.Comments.Count)
            )
            .ForMember(
                d => d.AssignedTasks,
                m => m.MapFrom(
                    s => s.MemberShips
                        .Where(x => x.Project.Memberships.Any(ms => ms.UserId == currentUserId))
                        .SelectMany(x => x.Project.Tasks)
                )
            );

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
            .ForMember(d => d.Assignees, m => m.MapFrom(s => s.Assignees.Select(x => x.MemberShip)));
        CreateProjection<Task, TaskDto>()
            .ForMember(d => d.Status, m => m.MapFrom(s => s.Status.Name))
            .ForMember(d => d.Assignees, m => m.MapFrom(s => s.Assignees.Select(x => x.MemberShip)));

        // Status
        CreateProjection<Status, StatusDto>();

        // Comment
        CreateProjection<Comment, CommentDto>();

        #endregion

        #region Maps

        // User
        CreateMap<UpdateUserDto, User>();

        // Project
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();

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
        CreateMap<CreateTeamDto, Team>();
        CreateMap<UpdateTeamDto, Team>();

        #endregion
    }
}