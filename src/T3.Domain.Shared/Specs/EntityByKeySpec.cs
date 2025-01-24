namespace T3.Domain.Shared.Specs;

using System.Linq.Expressions;

public record EntityByKeySpec<TEntity, TKey> : Specification<TEntity> 
    where TEntity : IHasId<TKey>
    where TKey : IEquatable<TKey>
{
    public required TKey Id { get; init; }

    public override Expression<Func<TEntity, bool>> ToExpression() 
        => c => c.Id.Equals(this.Id);
}