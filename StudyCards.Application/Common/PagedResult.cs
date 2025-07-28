namespace StudyCards.Application.Common;

public class PagedResult<T>(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
{
    public IReadOnlyList<T> Items { get; init; } = items;
    public int TotalCount { get; init; } = totalCount;
    public int PageNumber { get; init; } = pageNumber;
    public int PageSize { get; init; } = pageSize;

    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}