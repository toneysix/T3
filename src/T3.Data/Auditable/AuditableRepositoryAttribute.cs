namespace T3.Data.Auditable;

/// <summary>
/// Атрибут, указывающий, что данный репозиторий включает в себя методы получения прикладной информации к сущности.
/// </summary>
/// <remarks>На базе этого атрибута строится новый класс, реализующий IAuditable интерфейс.</remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AuditableRepositoryAttribute : Attribute
{
}