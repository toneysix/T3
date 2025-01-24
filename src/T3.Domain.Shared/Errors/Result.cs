namespace T3.Domain.Shared.Errors;

/// <summary>
/// Результат выполнения операции.
/// </summary>
public class Result
{
    /// <summary>
    /// Получает состояние успеха операции.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Получает состояние неуспеха операции.
    /// </summary>
    public bool IsFailure => !this.IsSuccess;

    /// <summary>
    /// Получает ошибку в выполнении операции.
    /// </summary>
    public Error Error { get; }

    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException();

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException();

        this.IsSuccess = isSuccess;
        this.Error = error;
    }

    /// <summary>
    /// Создает результат операции с успешным выполнением.
    /// </summary>
    /// <returns>Возвращает результат с успешным выполнением</returns>
    public static Result Success() => new(true, Error.None);

    /// <summary>
    /// Создает результат <typeparamref name="TValue"/> с успешным выполнением.
    /// </summary>
    /// <typeparam name="TValue">Результат операции</typeparam>
    /// <param name="value">Результат операции</param>
    /// <returns>Возвращает результат <typeparamref name="TValue"/> с успешным выполнением</returns>
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    /// <summary>
    /// Создает результат с неуспешным выполнением с указанной ошибкой.
    /// </summary>
    /// <param name="error">Ошибка, возникшая в процессе выполнения операции</param>
    /// <returns>Возвращает результат с неуспешным выполнением с указанной ошибкой</returns>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>
    /// Создает результат <typeparamref name="TValue"/> с неуспешным выполнением и указанной ошибкой.
    /// </summary>
    /// <typeparam name="TValue">Результат операции</typeparam>
    /// <param name="error">Ошибка, возникшая в процессе выполнения операции</param>
    /// <returns>Возвращает результат <typeparamref name="TValue"/> с неуспешным выполнением и указанной ошибкой</returns>
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    /// <summary>
    /// Создает результат <typeparamref name="TValue"/> с успехом, зависящим от <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="TValue">Результат операции</typeparam>
    /// <param name="value">Результат операции</param>
    /// <returns>Возвращает результат <typeparamref name="TValue"/> с успехом в случае, если <paramref name="value"/> имеет значение.
    /// В противном случае возвращает неуспешный результат с ошибкой <see cref="Error.NullValue">пустое значение</see></returns>
    public static Result<TValue> Create<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static implicit operator Result(Error error) => Result.Failure(error);
}