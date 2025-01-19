using System;
using Core.Entities;

namespace Core.Interface;

public interface IGenericRepository<T> where T :BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllListAsync();
    Task<IReadOnlyList<T>> GetListAsync(ISpecification<T> specification);
    Task<IReadOnlyList<TResult>> GetListAsync<TResult>(ISpecification<T,TResult> specification);
    Task<T?> GetEntityWithSpec(ISpecification<T> specification);
     Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T,TResult> specification);
    
   
    void Add(T entity);
    void Delete(T entity);
    void Update(T entity);
    Task<bool> IsExistsAsync(int id);
    bool IsExists(int id);
    Task<int> CountAsync(ISpecification<T> specification);

}
