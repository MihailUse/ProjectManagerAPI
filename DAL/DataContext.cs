using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DAL
{
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

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        #region override SaveChanges
        public override int SaveChanges()
        {
            _saveChanges();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _saveChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _saveChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            _saveChanges();
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

            _configureSoftDeleteFilter(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void _configureSoftDeleteFilter(ModelBuilder modelBuilder)
        {
            var softDeletableTypes = modelBuilder.Model.FindEntityTypes(typeof(ISoftDeletable));

            foreach (var softDeletableType in softDeletableTypes)
            {
                ParameterExpression parameter = Expression.Parameter(softDeletableType.ClrType);
                MemberExpression deletableProperty = Expression.Property(parameter, nameof(ISoftDeletable.DeletedAt));
                ConstantExpression nullableProperty = Expression.Constant(null);

                LambdaExpression queryFilter = Expression.Lambda(
                    Expression.Equal(deletableProperty, nullableProperty)
                );

                softDeletableType.SetQueryFilter(queryFilter);
            }
        }

        private void _saveChanges()
        {
            DateTime now = DateTime.UtcNow;

            foreach (var entity in ChangeTracker.Entries<ISoftDeletable>())
            {
                if (entity.State == EntityState.Deleted)
                {
                    entity.State = EntityState.Unchanged;
                    entity.Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = now;
                }
            }

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
}