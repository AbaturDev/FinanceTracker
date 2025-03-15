using FinanceTracker.Domain.Dtos.Transactions;
using FluentResults;

namespace FinanceTracker.Domain.Interfaces;

public interface ITransactionService
{
    Task<Result<int>> CreateTransactionAsync(CreateTransactionDto dto, CancellationToken ct);
}