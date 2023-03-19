using Application.DTO.Common;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions;

public static class MappingExtensions
{
    public static async Task<PaginatedList<TDestination>> ProjectToPaginatedList<TSource, TDestination>(
        this IQueryable<TSource> queryable,
        IConfigurationProvider configuration,
        PaginatedListQuery query
    )
        where TDestination : class
        where TSource : class
    {
        var queryableDestination = queryable
            .ProjectTo<TDestination>(configuration)
            .AsNoTracking();

        var count = await queryable.CountAsync();
        var items = queryable
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ProjectTo<TDestination>(configuration)
            .AsAsyncEnumerable();

        return new PaginatedList<TDestination>(items, count, query.PageNumber, query.PageSize);
    }
}