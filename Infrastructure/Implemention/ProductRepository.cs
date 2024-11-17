using System;
using Core.Entities;
using Core.Interface;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implemention;

public class ProductRepository(ApplicationDbContext dbcontext) : IProductRepository
{
    public void AddProduct(Product product)
    {
        dbcontext.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        dbcontext.Products.Remove(product);
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await dbcontext.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand ,string? type,string? sort)
    {
        var query = dbcontext.Products.AsQueryable();
      
        if(!string.IsNullOrWhiteSpace(brand))
            query = query.Where(x=>x.Brand==brand);
        if(!string.IsNullOrWhiteSpace(type))
            query = query.Where(x=>x.Type==type);
        if(!string.IsNullOrWhiteSpace(sort))
        {
            //query.OrderBy()
            query = sort switch
            {
                "priceAsc" => query.OrderBy(x=>x.Price),
                "priceDsc" => query.OrderByDescending(x=>x.Price),
                _ => query.OrderBy(x=>x.Name )
            };
        }
        return await query.ToListAsync();
    }

    public async Task<bool> ProductIsExistsAsync(int id)
    {
       return await dbcontext.Products.AsNoTracking().AnyAsync(x=>x.Id==id);
    }

    
    public bool ProductIsExist(int id)
    {
        return dbcontext.Products.Any(x=>x.Id ==id);
    }
    public void UpdateProduct(Product product)
    {
        dbcontext.Entry(product).State = EntityState.Modified;
        dbcontext.Products.Update(product);
    }
    public async Task<bool> SaveChangesAsync()
    {
       return await dbcontext.SaveChangesAsync()>0;
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await dbcontext.Products.AsNoTracking().Select(x=>x.Brand)
        .Distinct()
        .ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypeAsync()
    {
        return await dbcontext.Products.AsNoTracking().Select(x=>x.Type)
        .Distinct()
        .ToListAsync();
    }
}
