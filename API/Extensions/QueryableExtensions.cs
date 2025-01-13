using System;
using System.Linq.Expressions;

namespace API.Extensions;

public static  class QueryableExtensions
{
    public static IQueryable<T> WhereDynmic<T>(this IQueryable<T> source, string propertyName, string value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var constant = Expression.Constant(value);

        var propertyType = property.Type;
        var convertedValue = Convert.ChangeType(value, propertyType);

        // Create the expression x.Property == value
        var equalExpression = Expression.Equal(property, Expression.Constant(convertedValue));
        
        var lambda = Expression.Lambda<Func<T, bool>>(equalExpression, parameter);
        return source.Where(lambda);    
    }
}
