using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IDatabaseContext
{
    DbSet<User> Users { get; }
    DbSet<UserSession> UserSessions { get; }
    DbSet<Project> Projects { get; }
    DbSet<MemberShip> MemberShips { get; }
    DbSet<Comment> Comments { get; }
    DbSet<Assignee> Assignees { get; }
    DbSet<Status> Statuses { get; }
    DbSet<Domain.Entities.Task> Tasks { get; }
    DbSet<Team> Teams { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}