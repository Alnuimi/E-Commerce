using System;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CartController(ICartService cartService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<ShoppingCart>> GetCartAsync(string id)
    {
        ShoppingCart? data = await cartService.GetCartAsync(id);
        return Ok(data ?? new ShoppingCart{Id= id});
    }

    [HttpPost]
    public async Task<ActionResult<ShoppingCart>> UpdateCartAsync(ShoppingCart cart)
    {
        ShoppingCart? data = await cartService.SetCartAsync(cart);

        if(data == null) return BadRequest("Proplem updateing cart");

        return data;
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteCartAsync(string id )
    {
        bool result = await cartService.DeleteCartAsync(id);
        if(!result) return BadRequest("Proplem deleteing cart");
        return Ok();
    }

}
