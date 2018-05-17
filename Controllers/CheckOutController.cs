using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyStore.Migrations;
using MyStore.Models;

namespace MyStore.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly BoatChartersDbContext _oContext;
        public CheckOutController(BoatChartersDbContext oContext)
        {
            _oContext = oContext;
        }
   
        public IActionResult Index()
        {
            return View();
        }
     

        [HttpPost]
        public IActionResult Index(CheckoutViewModel model)
        {
            Order order = null;
            if (ModelState.IsValid)
            {
                order = _oContext.Orders.Include(orders => orders.OrderItems)
                    .ThenInclude(orderitems => orderitems.Yacht).FirstOrDefault();
            }
            if (order == null)
            {
                order = new Order();
            }

            return View();
        }




    }
}