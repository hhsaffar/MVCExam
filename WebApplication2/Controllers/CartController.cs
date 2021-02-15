using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CartController : Controller
    {
        CartHandler cartHandler;
        public IActionResult Index()
        {
            cartHandler = new CartHandler(HttpContext);
            return View(cartHandler.GetCart().Orders);
        }

        
        [HttpPost]
        public IActionResult Add(int id, int count)
        {
            Cart c = HttpContext.ReadFromSession<Cart>("CART");

            return View();
        }

    }
}
