namespace T3.Domain.Shared.Specs;

/// <summary>
/// Описывает сущность предметной области, имеющую одну уникальную особенность.
/// </summary>
/// <typeparam name="TKey">Тип особенности</typeparam>
public interface IHasId<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Уникальная особенность сущности.
    /// </summary>
    TKey Id { get; }
}
