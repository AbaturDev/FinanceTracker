namespace FinanceTracker.Client.Dtos.ApiRequests;

public sealed record RequestResult(bool IsSuccess, string ErrorMessage = "");