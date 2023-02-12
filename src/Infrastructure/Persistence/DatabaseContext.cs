using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Persistence;

public class DatabaseContext : DbContext, IDatabaseContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<MemberShip> MemberShips => Set<MemberShip>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Assignee> Assignees => Set<Assignee>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Status> Statuses => Set<Status>();
    public DbSet<Domain.Entities.Task> Tasks => Set<Domain.Entities.Task>();
    public DbSet<Team> Teams => Set<Team>();

    private readonly SaveChangesInterceptor _saveChangesInterceptor;

    public DatabaseContext(DbContextOptions<DatabaseContext> options, SaveChangesInterceptor interceptor) : base(options)
    {
        _saveChangesInterceptor = interceptor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignee>().HasKey(x => new { x.UserId, x.TaskId });
        modelBuilder.Entity<MemberShip>().HasKey(x => new { x.UserId, x.ProjectId });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_saveChangesInterceptor);
    }
}
