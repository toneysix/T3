namespace T3.Domain.Shared.Errors;

using T3.Domain.Shared.Entities;

/// <summary>
/// Возможные варианты ошибок предметной области.
/// </summary>
public static class Errors
{
    /// <summary>
    /// Ошибки, связанные с сущностями предметной области.
    /// </summary>
    public static class Entities
    {
        public static Error NotFound<TEntity>()
            where TEntity : Entity
            => new ($"{typeof(TEntity).Name}.NotFound", $"{typeof(TEntity).Name} isn't found");

        public static Error AlreadyExists<TEntity>()
            where TEntity : Entity
             => new($"{typeof(TEntity).Name}.AlreadyExists", $"{typeof(TEntity).Name} already exists");
    }
}
