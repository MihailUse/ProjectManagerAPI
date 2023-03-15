using Application.DTO.Common;

namespace Application.DTO.Task;

public record SearchTaskDto : PaginatedListQuery
{
    public string? Search { get; set; }
    public Guid? StatusId { get; set; }
}