namespace T3.API.Shared.Abstract;

public interface IUseCase<TRequest, TResponse>
    where TRequest: Request
    where TResponse : Response
{
    /// <summary>
    /// Выполнить.
    /// </summary>
    /// <param name="request">Входные данные <typeparamref name="TRequest"/> необходимые для выполнения.</param>
    /// <returns>Возвращает результат <typeparamref name="TResponse"/> выполнения</returns>
    TResponse Use(TRequest request);

    /// <summary>
    /// Выполнить.
    /// </summary>
    /// <param name="request">Входные данные <typeparamref name="TRequest"/> необходимые для выполнения.</param>
    /// <returns>Возвращает результат <typeparamref name="TResponse"/> выполнения</returns>
    Task<TResponse> UseAsync(TRequest request);
}