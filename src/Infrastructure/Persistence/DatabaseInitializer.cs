using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class DatabaseInitializer
{
    private readonly DatabaseContext _context;

    public DatabaseInitializer(DatabaseContext context)
    {
        _context = context;
    }

    public void Initialise()
    {
        _context.Database.MigrateAsync().Wait();
    }

    public void SeedData()
    {
        if (!_context.Statuses.Any())
        {
            var statuses = new[]
            {
                new Status(Statuses.Todo),
                new Status(Statuses.InProcessing),
                new Status(Statuses.Done),
            };

            _context.Statuses.AddRange(statuses);
        }

        if (!_context.Roles.Any())
        {
            var roles = new[]
            {
                new Role(Roles.Owner),
                new Role(Roles.MemberShip)
            };

            _context.Roles.AddRange(roles);
        }

        _context.SaveChanges();
    }
}