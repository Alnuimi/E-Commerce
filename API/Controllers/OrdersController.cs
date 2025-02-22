using System;
using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interface;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrdersController(ICartService cartService, IUnitOfWork unit) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto createOrderDto)
    {
        var email = User.GetEmail();
        var cart = await cartService.GetCartAsync(createOrderDto.CartId);
        if(cart == null) return BadRequest("Cart not found");
        if(cart.PaymentIntentId == null) return BadRequest("No payment intent for this order");
        var items = new List<OrderItem>();
        foreach (var item in cart.Items)
        {
            var productItem = await unit.Repository<Product>().GetByIdAsync(item.ProductId);
            if(productItem == null) return BadRequest("Problem with the order");

            var itemOrder = new ProductItemOrdered
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                PictureUrl = item.PictureUrl    
            };
            
            var orderItem = new OrderItem 
            {
                ItemOrdered = itemOrder,
                Price = productItem.Price,
                Quantity = item.Quantity
            };
            items.Add(orderItem);
        }

        var deliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync(createOrderDto.DeliveryMethodId);
        if(deliveryMethod == null) return BadRequest("No delivery method selected");
        var order = new Order
        {
            OrderItems = items,
            DeliveryMethod = deliveryMethod,
            ShippingAddress = createOrderDto.ShippingAddress,
            SubTotal = items.Sum(x => x.Price * x.Quantity),
            PaymentSummary = createOrderDto.PaymentSummary,
            PaymentIntentId = cart.PaymentIntentId,
            BuyerEmail = email
        };

        unit.Repository<Order>().Add(order);

        
        if(await unit.Complate()) 
        {
            //var orderDto = order.ToDto();
            return order;
        }

        return BadRequest("Problem creating order.");
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
    {

        var spec = new OrderSpecification(User.GetEmail());
        var orders = await unit.Repository<Order>().GetListAsync(spec);
       if(orders == null) return NotFound();
        var ordersDto = orders.Select(x => x.ToDto()).ToList(); 
        return Ok(ordersDto);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(int id)
    {
        var spec = new OrderSpecification(User.GetEmail(), id);
        var order = await unit.Repository<Order>().GetEntityWithSpec(spec);
        if(order == null) return NotFound();
        return Ok(order.ToDto());
    }
}
