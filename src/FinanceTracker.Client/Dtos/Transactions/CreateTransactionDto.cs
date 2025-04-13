namespace FinanceTracker.Client.Dtos.Transactions;

public sealed record CreateTransactionDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Amount { get; set; }
}