namespace FinanceTracker.Application.Common.Pagination;

public record PageQueryFilter
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
}