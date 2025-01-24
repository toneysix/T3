namespace T3.Transactions.Core.Errors;

using T3.Domain.Shared.Errors;

/// <summary>
/// Описывает варианты ошибок предметной области.
/// </summary>
public static class Errors
{
    /// <summary>
    /// Ошибки транзакций.
    /// </summary>
    public static class Transaction
    {
        public static readonly Error MaxTransactionsExceeded = new(
            "Transaction.MaxExceeded",
            "Transactions count must not exceed 100");

        public static readonly Error DateTimeIsInFuture = new(
            "Transaction.DateTimeIsInFuture",
            "Date must not be in future");

        public static readonly Error AmountIsNotPositive = new(
            "Transaction.Amount",
            $"Amount must be positive");
    }
}
