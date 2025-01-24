namespace T3.Domain.Shared.Specs;

using System.Linq.Expressions;

public record EntityByGuidSpec<TEntity> : EntityByKeySpec<TEntity, Guid>
    where TEntity : IHasId<Guid>
{
    public override Expression<Func<TEntity, bool>> ToExpression()
        => c => c.Id == this.Id;
}