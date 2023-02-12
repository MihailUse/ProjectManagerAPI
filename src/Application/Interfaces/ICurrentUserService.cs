namespace Application.Interfaces;

public interface ICurrentUserService
{
    Guid GetUserId(bool isRequired = true);
}
