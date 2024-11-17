using System;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T :BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query,ISpecification<T> specification)
    {
        if(specification.Expression!=null)
        {
            query = query.Where(specification.Expression);
        }
        if(specification.OrderBy !=null )
        { 
            query =query.OrderBy(specification.OrderBy);
        }
        if(specification.OrderByDescending !=null )
        { 
            query =query.OrderByDescending(specification.OrderByDescending);
        }
        return query;
    }

       public static IQueryable<TResult> GetQuery<TSpec,TResult>(IQueryable<T> query,ISpecification<T,TResult> specification)
    {
        if(specification.Expression!=null)
        {
            query = query.Where(specification.Expression);
        }
        if(specification.OrderBy !=null )
        { 
            query =query.OrderBy(specification.OrderBy);
        }
        if(specification.OrderByDescending !=null )
        { 
            query =query.OrderByDescending(specification.OrderByDescending);
        }
        var selectQuery = query as IQueryable<TResult>;
        if(specification.Select!=null)
        {
            selectQuery = query.Select(specification.Select);
        }
        if(specification.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct();
        }
        return selectQuery ?? query.Cast<TResult>();
    }
    
}
