using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Core.Interface;

public interface ISpecification<T>
{
    Expression<Func<T,bool>>? Expression{get;}
    Expression<Func<T,object >>?  OrderBy{get;}
    Expression<Func<T,object >>?  OrderByDescending{get;}
    List<Expression<Func<T, object>>> Includes {get;}
    List<string> IncludesStrings {get;} // For ThenInclude
    bool IsDistinct {get;}
    int Take{get;}
    int Skip{get;}
    bool IsApplyPagingEnabled{get;}
    IQueryable<T> ApplyExpression(IQueryable<T> query);

}
public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>>? Select {get;}
}
