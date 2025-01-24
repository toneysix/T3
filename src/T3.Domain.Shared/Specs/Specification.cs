namespace T3.Domain.Shared.Specs;

using System;
using System.Linq.Expressions;

public abstract record Specification<T>
{
    public static implicit operator Expression<Func<T, bool>>(Specification<T> s) => s.ToExpression();

    public abstract Expression<Func<T, bool>> ToExpression();
}