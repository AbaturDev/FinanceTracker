namespace FinanceTracker.Domain.Dtos.SavingGoals;

public class UpdateSavingGoalDto
{
    public string? Name { get; set; }
    public decimal? CurrentBalance { get; set; }
    public decimal? AmountOfMoney { get; set; }
    public string? Goal { get; set; }
    public DateOnly? DueDate { get; set; }
}