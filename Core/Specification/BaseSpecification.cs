using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Core.Interface;

namespace Core.Specification;

public class BaseSpecification<T>( Expression<Func<T, bool>>?  expression) : ISpecification<T>
{
    //SpecificationEvaluator
    protected BaseSpecification():this(null)
    {
        
    }
    public Expression<Func<T, bool>>? Expression => expression;
    public  Expression<Func<T,object >>?  OrderBy{get;private set;}
    public Expression<Func<T,object >>?  OrderByDescending{get;private set;}

    public bool IsDistinct  {get;private set;}

    public int Take {get;private set;}

    public int Skip {get;private set;}
    public bool IsApplyPagingEnabled{get;private set;}
    protected void AddOrderBy ( Expression<Func<T,object >> OrderByExpression)
    {
        OrderBy =OrderByExpression;
    }
    protected void  AddOrderByDescending(Expression<Func<T,object >> OrderByDescendingExpression)
    {
        OrderByDescending =OrderByDescendingExpression;
    }

    protected void ApplyDistinct()
    {
        IsDistinct=true;
    }

    protected void ApplyPaging(int skip,int take)
    {
        Skip=skip;
        Take=take;
        IsApplyPagingEnabled=true;
    }
    protected void AddSkip(int skip)
    {
        Skip=skip;
    }

    public IQueryable<T> ApplyExpression(IQueryable<T> query)
    {
        if(Expression!=null)
        {
            query =query.Where(Expression);
        }
        return query;
    }
}

public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? expression) : BaseSpecification<T>(expression), ISpecification<T, TResult>
{
    protected BaseSpecification():this(null)
    {
        
    }
    public Expression<Func<T, TResult>>? Select {get;private set;}

    protected void AddSelect(Expression<Func<T,TResult>> selectExperssion)
    {
        Select=selectExperssion;
    }
}