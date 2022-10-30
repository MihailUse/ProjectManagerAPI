using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DAL
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Assignee> Assignees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Entities.Task> Tasks { get; set; }

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
            IEnumerable<EntityEntry> entities = ChangeTracker.Entries();
            DateTime now = DateTime.Now;

            foreach (var entity in entities)
            {
                Type type = entity.GetType();

                switch (entity.State)
                {
                    case EntityState.Deleted:
                        if (type.IsAssignableFrom(typeof(ISoftDeletable)))
                        {
                            entity.State = EntityState.Unchanged;
                            entity.Property(nameof(ISoftDeletable.DeletedAt)).CurrentValue = now;
                        }
                        break;

                    case EntityState.Modified:
                        if (type.IsAssignableFrom(typeof(ITimestamp)))
                            entity.Property(nameof(ITimestamp.UpdatedAt)).CurrentValue = now;
                        break;

                    case EntityState.Added:
                        if (type.IsAssignableFrom(typeof(ITimestamp)))
                            entity.Property(nameof(ITimestamp.CreatedAt)).CurrentValue = now;
                        break;
                }
            }
        }
    }
}