using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Repositories;

public interface IAttachRepository
{
    Task<Attach?> FindById(Guid attachId);
    Task Add(Attach attach);
    Task<bool> CheckExists(Guid attachId);
}