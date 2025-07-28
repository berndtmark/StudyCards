using AutoMapper;
using StudyCards.Application.Common;

namespace StudyCards.Api.Mapper.Converters;

public class PagedResultConverter<TSource, TDestination> :
    ITypeConverter<PagedResult<TSource>, PagedResult<TDestination>>
{
    public PagedResult<TDestination> Convert(
        PagedResult<TSource> source,
        PagedResult<TDestination> destination,
        ResolutionContext context)
    {
        var mappedItems = context.Mapper.Map<IReadOnlyList<TDestination>>(source.Items);

        return new PagedResult<TDestination>(
            mappedItems,
            source.TotalCount,
            source.PageNumber,
            source.PageSize
        );
    }
}
