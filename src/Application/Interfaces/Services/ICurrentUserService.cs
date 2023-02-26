namespace Application.Interfaces.Services;

public interface ICurrentUserService
{
    Guid GetUserId(bool isRequired = true);
}