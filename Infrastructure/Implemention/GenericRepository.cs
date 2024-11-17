using System;
using Core.Entities;
using Core.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implemention;

public class GenericRepository<T>(ApplicationDbContext dbContext) : IGenericRepository<T> where T : BaseEntity
{
    #region Task T? to get async by id from entity in database
    public async Task<T?> GetByIdAsync(int id)
    {
        return await dbContext.Set<T>().FindAsync(id);
    }
    #endregion

    #region Task IReadOnlyList<T> to get all async from entity in database
    public async Task<IReadOnlyList<T>> GetAllListAsync()
    {
        return await dbContext.Set<T>().ToListAsync();
    }
    #endregion


    public async Task<IReadOnlyList<T>> GetListAsync(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
    }

    public async Task<IReadOnlyList<TResult>> GetListAsync<TResult>(ISpecification<T, TResult> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
    }

    public async Task<T?> GetEntityWithSpec(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    #region Void to add entity to database 
    public void Add(T entity)
    {
        dbContext.Set<T>().Add(entity);
    }
    #endregion

    #region Void to delete from entity in database 
    public void Delete(T entity)
    {
        dbContext.Set<T>().Remove(entity);
    }
    #endregion
    #region Void to update entity in database

    public void Update(T entity)
    {
        dbContext.Set<T>().Attach(entity);
        dbContext.Entry(entity).State = EntityState.Modified;
    }
    #endregion

    #region Variable Task Boolean to check async if id is exists in entity
    public async Task<bool> IsExistsAsync(int id)
    {
        return await dbContext.Set<T>().AnyAsync(x => x.Id == id);
    }
    #endregion
    #region Variable Boolean to check if id is exists in entity
    public bool IsExists(int id)
    {
        return dbContext.Set<T>().Any(x => x.Id == id);
    }
    #endregion



    #region Task Boolean to save async change in database
    public async Task<bool> SaveChangeAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
    #endregion



    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator<T>.GetQuery(dbContext.Set<T>().AsQueryable<T>(), specification);
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        return SpecificationEvaluator<T>.GetQuery<T, TResult>(dbContext.Set<T>().AsQueryable<T>(), specification);
    }


}
