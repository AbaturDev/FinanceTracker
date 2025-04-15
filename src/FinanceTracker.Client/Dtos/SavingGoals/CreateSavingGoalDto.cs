namespace FinanceTracker.Client.Dtos.SavingGoals;

public sealed record CreateSavingGoalDto
{
    public string? Name { get; set; }
    public decimal? AmountOfMoney { get; set; }
    public string? Goal { get; set; }
    public string? DueDate { get; set; }
    public string? CurrencyCode { get; set; }
}