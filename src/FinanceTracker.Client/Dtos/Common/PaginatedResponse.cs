namespace FinanceTracker.Client.Dtos.Common;

public sealed record PaginatedResponse<T>
{
    public PaginatedResponse()
    {
        
    }
    public PaginatedResponse(ICollection<T> items, int pageNumber, int pageSize, int totalItemsCount)
    {
        Items = items;
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
        TotalItemsCount = totalItemsCount;
    }

    public ICollection<T> Items { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalItemsCount { get; set; }
}