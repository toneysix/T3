namespace T3.Domain.Shared.Repositories;

using T3.Domain.Shared.Entities;
using T3.Domain.Shared.Specs;

/// <summary>
/// Хранилище сущностей <typeparamref name="TEntity"/>
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
public interface IEntityRepository<TEntity> 
    where TEntity : Entity
{
    /// <summary>
    /// Добавляет <typeparamref name="TEntity"/> в хранилище.
    /// </summary>
    /// <param name="entity"><typeparamref name="TEntity"/>, которую необходимо добавить в хранилище</param>
    void Add(TEntity entity);

    /// <summary>
    /// Получает <typeparamref name="TEntity"/> по указанной спецификации.
    /// </summary>
    /// <param name="spec">Спецификация, по которой нужно получить <typeparamref name="TEntity"/></param>
    /// <returns>Возвращает <typeparamref name="TEntity"/>, соответствующую указанной спецификации</returns>
    TEntity? Get(Specification<TEntity> spec);

    /// <summary>
    /// Считает количество <typeparamref name="TEntity"/> по указанной спецификации.
    /// </summary>
    /// <param name="spec">Спецификация, по которой нужно посчитать количество <typeparamref name="TEntity"/></param>
    /// <returns>Возвращает количество <typeparamref name="TEntity"/>, соответствующее указанной спецификации</returns>
    long Count(Specification<TEntity> spec);

    /// <summary>
    /// Считает количество всех <typeparamref name="TEntity"/>.
    /// </summary>
    /// <returns>Возвращает количество всех <typeparamref name="TEntity"/></returns>
    long CountAll();

    /// <inheritdoc cref="Add(TEntity)"/>
    Task AddAsync(TEntity entity);

    /// <inheritdoc cref="Get(Specification{TEntity})"/>
    Task<TEntity> GetAsync(Specification<TEntity> spec);

    /// <inheritdoc cref="Count(Specification{TEntity})"/>
    Task<long> CountAsync(Specification<TEntity> spec);

    /// <inheritdoc cref="CountAll"/>
    Task<long> CountAllAsync();
}
