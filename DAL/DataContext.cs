using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DAL;

public class DataContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<MemberShip> MemberShips { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Assignee> Assignees { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Status> Statuses { get; set; } = null!;
    public DbSet<Entities.Task> Tasks { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    #region override SaveChanges
    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        AddTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(o => o.MigrationsAssembly("API"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignee>().HasKey(x => new { x.UserId, x.TaskId });
        modelBuilder.Entity<MemberShip>().HasKey(x => new { x.UserId, x.ProjectId });
    }

    private void AddTimestamps()
    {
        var now = DateTime.UtcNow;

        // set deletion time 
        foreach (var entity in ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entity.State == EntityState.Deleted)
            {
                entity.State = EntityState.Unchanged;
                entity.Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = now;
            }
        }

        // set creation or updation time
        foreach (var entity in ChangeTracker.Entries<ITimestamp>())
        {
            switch (entity.State)
            {
                case EntityState.Modified:
                    entity.Property(nameof(ITimestamp.UpdatedAt)).CurrentValue = now;
                    break;

                case EntityState.Added:
                    entity.Property(nameof(ITimestamp.CreatedAt)).CurrentValue = now;
                    entity.Property(nameof(ITimestamp.UpdatedAt)).CurrentValue = now;
                    break;
            }
        }
    }
}