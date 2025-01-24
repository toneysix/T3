namespace T3.Transactions.API.Contracts;

using Mapster;
using T3.Transactions.API.UseCases.CommitTransaction;
using T3.Transactions.API.UseCases.GetTransaction;
using T3.Transactions.Core.Entities;

public sealed class MapperConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Transaction, TransactionGetResponse>();
        config.NewConfig<TransactionCommitRequest, Transaction>();
        config.NewConfig<DateTime, TransactionCommitResponse>()
            .Map(dest => dest.InsertDateTime, src => src);
    }
}
