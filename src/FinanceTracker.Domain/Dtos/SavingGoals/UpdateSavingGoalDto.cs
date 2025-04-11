namespace FinanceTracker.Domain.Dtos.SavingGoals;

public sealed record UpdateSavingGoalDto
{
    public string? Name { get; set; }
    public decimal? AmountOfMoney { get; set; }
    public string? Goal { get; set; }
    public DateOnly? DueDate { get; set; }
}