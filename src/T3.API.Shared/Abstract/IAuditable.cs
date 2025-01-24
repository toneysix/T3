namespace T3.API.Shared.Abstract;

using T3.Domain.Shared.Entities;
using T3.Domain.Shared.Specs;

public interface IAuditable<TEntity>
    where TEntity : Entity
{
    /// <summary>
    /// Получает дополнительную информацию о <typeparamref name="TEntity"/>.
    /// </summary>
    /// <param name="specification">Спецификация, по которой нужно извлечь дополнительную информацию о <typeparamref name="TEntity"/></param>
    /// <returns>Возвращает дополнительную информацию о <typeparamref name="TEntity"/></returns>
    AuditableEntity Get(Specification<TEntity> specification);
}