using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync (ApplicationDbContext dbContext)
    {
        if(!dbContext.Products.Any())
        {
            var productData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
            var product = JsonSerializer.Deserialize<List<Product>>(productData);
            if(product==null)return;
            dbContext.Products.AddRange(product);
            await dbContext.SaveChangesAsync();
        }

        if(!dbContext.DeliveryMethods.Any())
        {
            var deliveryMethodData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");
            var deliveryMethod = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodData);
            if(deliveryMethod==null)return;
            dbContext.DeliveryMethods.AddRange(deliveryMethod);
            await dbContext.SaveChangesAsync();
        }
    }
}
