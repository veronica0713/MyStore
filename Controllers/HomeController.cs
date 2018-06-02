using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;

namespace MyStore.Controllers
{
    public class HomeController : Controller
    {
         BoatChartersDbContext _db;

        public HomeController(BoatChartersDbContext boatChartersDbContext)
        {
            _db = boatChartersDbContext;
        }
             
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Car()
        {
            return View();
            //return Json("My method");
        }
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> CartSummary()
        {
            Guid cartId;
            Cart cart = null;
            if (Request.Cookies.ContainsKey("cartId"))
            {
                if (Guid.TryParse(Request.Cookies["cartId"], out cartId))
                {
                    cart = await _db.Carts
                        .Include(carts => carts.CartItems)
                        .ThenInclude(cartitems => cartitems.Yacht)
                        .FirstOrDefaultAsync(x => x.CookieIdentifier == cartId);
                }
            }
            if (cart == null)
            {
                cart = new Cart();
            }
            return Json(cart);
        }
        public IActionResult Search(string search)
        {
            var mo =_db.Products.Where(x => x.Description.Contains(search) || x.Name.Contains(search)).ToList();
            return View("Search", mo);
        }
    }
}
