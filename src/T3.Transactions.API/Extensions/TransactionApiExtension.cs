namespace T3.Transactions.API;

using Microsoft.Extensions.DependencyInjection;
using T3.Transactions.Core.Services;

public static class TransactionApiExtension
{
    public static IServiceCollection AddTransactionAPI(
        this IServiceCollection services)
    {
        return services.AddUseCases(typeof(TransactionApiExtension).Assembly)
            .AddScoped<ITransactionService, TransactionService>();
    }
}
