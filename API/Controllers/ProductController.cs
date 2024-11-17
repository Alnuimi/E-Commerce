using System;
using Core.Entities;
using Core.Interface;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IGenericRepository<Product> repository) :ControllerBase
{
   

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand ,string? type,string? sort){

        var spec = new ProductSpecification(brand:brand,type:type,sort:sort);
        var products = await repository.GetListAsync(spec);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id )
    {
        var product = await repository.GetByIdAsync(id);
        if(product ==null) return NotFound();
        return product;
    }
    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        repository.Add(product);
        if( await repository.SaveChangeAsync())
        {
            return CreatedAtAction("GetProduct",new{id= product.Id},product );
        }
        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id ,Product product)
    {
        if(id != product.Id || !ProductIsExist(id)) return BadRequest("Can't update this product");

        repository.Update(product);
        if(await repository.SaveChangeAsync())
        {
            return NoContent();
        }
        return BadRequest("Proplem updating the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await repository.GetByIdAsync(id);
        if(product==null ) return NotFound();
        repository.Delete(product);
        if(await repository.SaveChangeAsync()){
            return NoContent();
        }
       //repository.DeleteProduct()
       return BadRequest("Proplem deleting the product");
    }   
    
    [HttpGet("brands")]

    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        //await repository.GetBrandsAsync()
        var spec = new BrandSpecification();

        return Ok(await repository.GetListAsync(spec));
    }

    [HttpGet("types")]

    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        //await repository.GetTypeAsync()
        var spec = new TypeSpecification();

        return Ok(await repository.GetListAsync(spec));
    }
    private bool ProductIsExist(int id )
    {
        return repository.IsExists(id);
    }

}
