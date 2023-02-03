using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Mappings;

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> ProjectToPaginatedListAsync<TDestination>(
        this IQueryable queryable,
        IConfigurationProvider configuration,
        int pageNumber,
        int pageSize
    ) where TDestination : class
    {
        var queryableDestination = queryable
            .ProjectTo<TDestination>(configuration)
            .AsNoTracking();

        return PaginatedList<TDestination>.CreateAsync(queryableDestination, pageNumber, pageSize);
    }

    public static Task<List<TDestination>> ProjectToListAsync<TDestination>(
        this IQueryable queryable,
        IConfigurationProvider configuration
    ) where TDestination : class
    {
        return queryable
            .ProjectTo<TDestination>(configuration)
            .AsNoTracking()
            .ToListAsync();
    }
}
