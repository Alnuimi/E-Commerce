using System;
using System.Collections.Concurrent;
using Core.Entities;
using Core.Interface;
using Infrastructure.Implemention;

namespace Infrastructure.Data;

public class UnitOfWork (ApplicationDbContext dbContext): IUnitOfWork
{
    private readonly ConcurrentDictionary<string, object> _repositories = new();
    public async Task<bool> Complate()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }

    public void Dispose()
    {
        dbContext.Dispose();
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        var type = typeof(TEntity).Name;
        return (IGenericRepository<TEntity>) _repositories.GetOrAdd(type, t => 
        {
            var repositoryType =  typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));
            return Activator.CreateInstance(repositoryType, dbContext)
                ?? throw new InvalidOperationException($"Could not create repository instance for {t}");
        });
    }
}
