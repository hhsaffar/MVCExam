using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2
{
    public  class CartHandler
    {

        HttpContext _HttpContext;
        public CartHandler(HttpContext httpContext)
        {
            _HttpContext = httpContext;
        }
        public int GetAlreadyOrderLineCount()
        {

            

            Cart cart = _HttpContext.ReadFromSession<Cart>("CART");

            if (cart == null)
            {
                return 0;
            }

            return cart.Orders.Count;
        }

        public int GetAlreadyOrderedCount(int? id)
        {
            if (id == null)
                return 0;

            Cart cart = _HttpContext.ReadFromSession<Cart>("CART");

            if (cart == null)
            {
                return 0;
            }

            var productOrderLine = cart.Orders.Where(o => o.OrderLineItem.ProductId == id).SingleOrDefault();
            if (productOrderLine == null)
            {
                return 0;
            }

            return productOrderLine.Count;
        }

        public Cart GetCart()
        {
            Cart cart = _HttpContext.ReadFromSession<Cart>("CART");

            if (cart == null)
            {
                cart = new Cart() { Orders = new List<OrderLine>() };
            }

            return cart;
        }

        internal void SaveCart(Cart cart)
        {
            _HttpContext.WriteToSession("CART", cart);
        }
    }
}
