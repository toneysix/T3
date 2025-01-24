namespace T3.Transactions.Core.Entities;

using System;
using T3.Domain.Shared.Entities;
using T3.Domain.Shared.Specs;

public record Transaction : Entity, IHasId<Guid>
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public virtual required Guid Id { get; init; }

    /// <summary>
    /// Дата и время транзакции.
    /// </summary>
    public virtual required DateTime Date { get; init; }

    /// <summary>
    /// Сумма транзакции.
    /// </summary>
    public virtual required decimal Amount { get; init; }
}
