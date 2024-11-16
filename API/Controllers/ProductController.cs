using System;
using Core.Entities;
using Core.Interface;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductRepository repository) :ControllerBase
{
   

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand ,string? type,string? sort){
        return Ok(await repository.GetProductsAsync(brand,type,sort));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id )
    {
        var product = await repository.GetProductByIdAsync(id);
        if(product ==null) return NotFound();
        return product;
    }
    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        repository.AddProduct(product);
        if( await repository.SaveChangesAsync())
        {
            return CreatedAtAction("GetProduct",new{id= product.Id},product );
        }
        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id ,Product product)
    {
        if(id != product.Id || !ProductIsExist(id)) return BadRequest("Can't update this product");

        repository.UpdateProduct(product);
        if(await repository.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Proplem updating the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await repository.GetProductByIdAsync(id);
        if(product==null ) return NotFound();
        repository.DeleteProduct(product);
        if(await repository.SaveChangesAsync()){
            return NoContent();
        }
       //repository.DeleteProduct()
       return BadRequest("Proplem deleting the product");
    }   
    
    [HttpGet("brands")]

    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repository.GetBrandsAsync());
    }

    [HttpGet("types")]

    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repository.GetTypeAsync());
    }
    private bool ProductIsExist(int id )
    {
        return repository.ProductIsExist(id);
    }

}
