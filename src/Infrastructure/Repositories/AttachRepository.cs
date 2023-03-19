using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Repositories;

public class AttachRepository : IAttachRepository
{
    private readonly DatabaseContext _database;

    public AttachRepository(DatabaseContext database)
    {
        _database = database;
    }

    public async Task<Attach?> FindById(Guid attachId)
    {
        return await _database.Attaches.FindAsync(attachId);
    }

    public async Task Add(Attach attach)
    {
        await _database.Attaches.AddAsync(attach);
        await _database.SaveChangesAsync();
    }

    public async Task<bool> CheckExists(Guid attachId)
    {
        return await _database.Attaches.AnyAsync(x => x.Id == attachId);
    }
}