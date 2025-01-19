using System;
using System.Runtime.Intrinsics.Arm;
using Core.Entities;
using Core.Interface;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services;

public class PaymentService(IConfiguration config,
     ICartService cartService,
     IUnitOfWork unit) : IPaymentServices
{
    public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
    {
        StripeConfiguration.ApiKey = config["StripSettings:SecretKey"];
        var cart = await cartService.GetCartAsync(cartId);
        if(cart == null) return null;

        decimal shoppingPrice = 0m;
        if(cart.DeliveryMethodId.HasValue)
        {
            var deliveryMethod =await unit.Repository<DeliveryMethod>().GetByIdAsync((int)cart.DeliveryMethodId);
            if(deliveryMethod == null) return null;

            shoppingPrice = deliveryMethod.Price;
            
        }

        foreach (var item in cart.Items)
        {
            var productItem = await unit.Repository<Core.Entities.Product>().GetByIdAsync(item.ProductId); 
            if(productItem == null ) return null;
            if(item.Price != productItem.Price)
            {
                item.Price = productItem.Price;
            }
        }

        var service = new PaymentIntentService();
        PaymentIntent? intent = null;
        if(string.IsNullOrEmpty(cart.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100)) + (long) (shoppingPrice * 100),
                Currency = "usd",
                PaymentMethodTypes = ["card"],
            };

             intent = await service.CreateAsync(options);
             cart.PaymentIntentId = intent.Id;
             cart.ClientSecret = intent.ClientSecret;
        }
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100)) + (long) (shoppingPrice * 100),
            };

            intent = await service.UpdateAsync(cart.PaymentIntentId, options);
            cart.PaymentIntentId = intent.Id;
            cart.ClientSecret = intent.ClientSecret;
            
        }

        await cartService.SetCartAsync(cart);

        return cart;

    }
}
