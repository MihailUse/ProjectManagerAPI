using Domain.Entities;

namespace Application.Interfaces.Services;

public interface IAttachService
{
    Task<Guid> GenerateImage();
    Task<Guid> SaveAttach(Attach attach, Stream fileStream);
    Task<Attach> GetById(Guid attachId);
    FileStream GetStream(Guid attachId);
}