namespace FinanceTracker.Domain.Dtos.SavingGoals;

public record CreateSavingGoalDto
{
    public required string Name { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal AmountOfMoney { get; set; }
    public required string Goal { get; set; }
    public DateOnly? DueDate { get; set; }
    public required string CurrencyCode { get; set; }
}