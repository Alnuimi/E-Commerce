using System;
using Core.Entities;
using Core.Interface;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.RequestHelper;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IUnitOfWork unit) :BaseApiController
{
   

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
        [FromQuery]ProductSpecParmas specParmas){

        var spec = new ProductSpecification(specParmas);
        return await CreatePagedResultAsync(unit.Repository<Product>(),spec,specParmas.PageIndex,specParmas.PageSize );
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id )
    {
        var product = await unit.Repository<Product>().GetByIdAsync(id);
         if(product ==null) return NotFound();
        return product;
    }
    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        unit.Repository<Product>().Add(product);
        if( await unit.Complate())
        {
            return CreatedAtAction("GetProduct",new{id= product.Id},product );
        }
        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id ,Product product)
    {
        if(id != product.Id || !ProductIsExist(id)) return BadRequest("Can't update this product");

        unit.Repository<Product>().Update(product);
        if(await unit.Complate())
        {
            return NoContent();
        }
        return BadRequest("Proplem updating the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await unit.Repository<Product>().GetByIdAsync(id);
        if(product==null ) return NotFound();
        unit.Repository<Product>().Delete(product);
        if(await unit.Complate()){
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

        return Ok(await unit.Repository<Product>().GetListAsync(spec));
    }

    [HttpGet("types")]

    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        //await repository.GetTypeAsync()
        var spec = new TypeSpecification();

        return Ok(await unit.Repository<Product>().GetListAsync(spec));
    }
    private bool ProductIsExist(int id )
    {
        return unit.Repository<Product>().IsExists(id);
    }

}
