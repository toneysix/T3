namespace T3.Domain.Shared.Errors;

/// <summary>
/// Описывает ошибку операции.
/// </summary>
public class Error : IEquatable<Error>
{
    /// <summary>
    /// Создает пустую ошибку.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    /// Создает ошибку с пустым значением.
    /// </summary>
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

    /// <summary>
    /// Код ошибки.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Сообщение ошибки.
    /// </summary>
    public string Message { get; }

    public Error(string code, string message)
    {
        this.Code = code;
        this.Message = message;
    }

    public static implicit operator string(Error error) => error.Code;

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Error? a, Error? b) => !(a == b);

    public virtual bool Equals(Error? other)
    {
        if (other is null)
            return false;

        return this.Code == other.Code && this.Message == other.Message;
    }

    public override bool Equals(object? obj) => obj is Error error && this.Equals(error);

    public override int GetHashCode() => HashCode.Combine(this.Code, this.Message);

    public override string ToString() => this.Code;
}