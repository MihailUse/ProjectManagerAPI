using Application.DTO.Project;
using Application.DTO.User;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<User, UserDto>();
        CreateMap<UpdateUserDto, User>();

        CreateProjection<User, UserBriefDto>();


        // Project
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();

        CreateProjection<Project, ProjectBriefDto>();
        CreateProjection<Project, ProjectDto>()
            .ForMember(d => d.TaskCount, m => m.MapFrom(s => s.Tasks.Count))
            .ForMember(d => d.MembershipCount, m => m.MapFrom(s => s.Memberships.Count));
    }
}