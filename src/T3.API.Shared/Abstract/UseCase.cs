namespace T3.API.Shared.Abstract;

using T3.Domain.Shared.Extensions;

public abstract class UseCase<TRequest, TResponse> : IUseCase<TRequest, TResponse>
    where TRequest: Request
    where TResponse: Response
{
    public abstract TResponse Use(TRequest request);

    public Task<TResponse> UseAsync(TRequest request)
    {
        return TaskExtension.ToAsync(() => this.Use(request));
    }
}
