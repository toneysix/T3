namespace T3.Transactions.Core.Services;

using T3.Domain.Shared.Errors;
using T3.Transactions.Core.Entities;

/// <summary>
/// Сервис транзакций.
/// </summary>
public interface ITransactionService
{
    /// <summary>
    /// Проводит транзакцию.
    /// </summary>
    /// <param name="transaction">Транзакция, которую необходимо провести.</param>
    /// <returns>Возвращает результат создания транзакции.</returns>
    Result Commit(Transaction transaction);

    /// <summary>
    /// Получает транзакцию по указанному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор, по которому необходимо получить транзакцию.</param>
    /// <returns>Возвращает транзакцию, соответствующую указанному идентификатору.</returns>
    Result<Transaction> GetBy(Guid id);

    /// <inheritdoc cref="Commit(Transaction)"/>
    Task<Result> CommitAsync(Transaction transaction);

    /// <inheritdoc cref="GetBy(Guid)"/>
    Task<Result<Transaction>> GetByAsync(Guid id);
}