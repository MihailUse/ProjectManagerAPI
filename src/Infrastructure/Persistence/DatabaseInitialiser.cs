using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class DatabaseInitialiser
{
    private readonly DatabaseContext _context;

    public DatabaseInitialiser(DatabaseContext context)
    {
        _context = context;
    }

    public void Initialise()
    {
        _context.Database.MigrateAsync().Wait();
    }

    public void SeedData()
    {
        // Default data
        // Seed, if necessary
        if (!_context.Statuses.Any())
        {
            var statuses = new[]{
                new Status("TODO"),
                new Status("In Processing"),
                new Status("Done"),
            };

            _context.Statuses.AddRange(statuses);
            _context.SaveChanges();
        }
    }
}
