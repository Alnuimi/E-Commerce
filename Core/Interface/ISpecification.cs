using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Core.Interface;

public interface ISpecification<T>
{
    Expression<Func<T,bool>>? Expression{get;}
    Expression<Func<T,object >>?  OrderBy{get;}
    Expression<Func<T,object >>?  OrderByDescending{get;}
    bool IsDistinct {get;}
}
public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>>? Select {get;}
}
