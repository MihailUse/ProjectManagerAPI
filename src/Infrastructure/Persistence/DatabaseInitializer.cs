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
        if (_context.Statuses.Any())
            return;

        var statuses = new[]
        {
            new Status("TODO"),
            new Status("In Processing"),
            new Status("Done"),
        };

        _context.Statuses.AddRange(statuses);
        _context.SaveChanges();
    }
}