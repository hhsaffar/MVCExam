using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopContext _context;
        private  CartHandler cartHandler;

        public HomeController(ShopContext context)
        {
            _context = context;
        }

        


        // GET: Home
        public async Task<IActionResult> Index()
        {
            cartHandler = new CartHandler(HttpContext);

            ViewData["AlreadyOrderedCount"] =cartHandler.GetAlreadyOrderLineCount();
            ViewData["Cats"] = _context.Products.Select(x => new CatListItem() { Id = x.Category, Name = x.Category }).Distinct().ToList();

            return View(await _context.Products.ToListAsync());
        }

        

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            cartHandler = new CartHandler(HttpContext);


            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["AlreadyOrderedCount"] = cartHandler.GetAlreadyOrderedCount(id);

            product.Images = _context.ProductImages.Where(x => x.ProductId == product.ProductId).ToList();

            return View(product);
        }

        

        [HttpPost]
        public async Task<IActionResult> Details(int id, int count)
        {
            cartHandler = new CartHandler(HttpContext);

            var cart = cartHandler.GetCart();

            var productOrderLine = cart.Orders.Where(o => o.OrderLineItem.ProductId == id).SingleOrDefault();
            if (productOrderLine == null)
            {
                productOrderLine = new OrderLine()
                {
                    OrderLineItem = _context.Products.Single(x => x.ProductId == id),
                    Count = 0
                };
                cart.Orders.Add(productOrderLine);
            }

            productOrderLine.Count = count;

            Predicate<OrderLine> negz = delegate (OrderLine x) { return x.Count<= 0; };

            cart.Orders.RemoveAll(negz);
            

            cartHandler.SaveCart(cart);

            return RedirectToAction("Index");
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,Category,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Category,Price")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }

    public class CatListItem
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
