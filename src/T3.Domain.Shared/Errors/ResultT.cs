namespace T3.Domain.Shared.Errors;

/// <summary>
/// Результат выполнения операции.
/// </summary>
/// <typeparam name="TValue">Результат выполнения операции.</typeparam>
public class Result<TValue> : Result
{
    private readonly TValue? value;

    /// <summary>
    /// Получает результат выполнения операции в случае её успеха.
    /// </summary>
    /// <exception cref="InvalidOperationException">The value of failure result cannot be null</exception>
    public TValue Value => this.IsSuccess
        ? this.value!
        : throw new InvalidOperationException("The value of failure result cannot be null.");

    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        this.value = value;
    }

    public static implicit operator Result<TValue>(TValue? value) => Result.Create(value);

    public static implicit operator Result<TValue>(Error error) => Result.Failure<TValue>(error);
}