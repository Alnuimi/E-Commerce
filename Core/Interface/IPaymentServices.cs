using System;
using Core.Entities;

namespace Core.Interface;

public interface IPaymentServices
{
    Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId);
}
