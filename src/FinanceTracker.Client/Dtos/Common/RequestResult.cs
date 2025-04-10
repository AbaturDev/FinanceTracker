namespace FinanceTracker.Client.Dtos.Common;

public sealed record RequestResult(bool IsSuccess, string ErrorMessage = "");