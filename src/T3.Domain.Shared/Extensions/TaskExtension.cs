namespace T3.Domain.Shared.Extensions;

public static class TaskExtension
{
    public static Task<TResult> ToAsync<TResult>(Func<TResult> func)
    {
        try
        {
            return Task.FromResult(func());
        }
        catch (Exception ex)
        {
            return Task.FromException<TResult>(ex);
        }
    }

    public static Task ToAsync(Action action)
    {
        try
        {
            return Task.FromResult(action);
        }
        catch (Exception ex)
        {
            return Task.FromException(ex);
        }
    }
}
