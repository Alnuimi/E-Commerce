using System;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController:ControllerBase
{
    private readonly ApplicationDbContext _dbcontext;
    public ProductController(ApplicationDbContext dbContext)
    {
        _dbcontext=dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(){
        return await _dbcontext.Products.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id )
    {
        var product = await _dbcontext.Products.FirstOrDefaultAsync(x=>x.Id ==id);
        if(product ==null) return NotFound();
        return product;
    }
    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        await _dbcontext.Products.AddAsync(product);
        await _dbcontext.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id ,Product product)
    {
        if(id != product.Id || ! await IsExistsProduct(id)) return BadRequest("Can't update this product");

        _dbcontext.Entry<Product>(product).State = EntityState.Modified;
        await _dbcontext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
       var product = await _dbcontext.Products.FindAsync(id);
       if(product==null) return NotFound();
       _dbcontext.Products.Remove(product);
       await _dbcontext.SaveChangesAsync();
       return NoContent();
    }   
    private async Task<bool> IsExistsProduct(int id)
    {
        return await _dbcontext.Products.AnyAsync(x=>x.Id ==id);
    }

}
