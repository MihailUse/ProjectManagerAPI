using Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Application.Interfaces.Services;

public interface IAttachService
{
    Task<Guid> GenerateImage();
    Task<Guid> SaveAttach(Attach attach, Stream fileStream);
    Task<Attach> GetById(Guid id);
    FileStream GetStream(Guid id);
    Task CheckAttachExists(Guid id);
}