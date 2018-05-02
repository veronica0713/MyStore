using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyStore.Models;

namespace MyStore.Controllers
{
    public class YachtController : Controller
    { 
    private List<Yacht> _yachts;

        public YachtController()
        {
            _yachts = new List<Yacht>();
            
            _yachts.Add(new Yacht
            {
                ID = 1,
                Name = "Road Bike",
                Description = "bla bla",
                Image = "images/cup.jpg",
                Size = 46,
                Price = 656.99m

            });
            _yachts.Add(new Yacht
            {
                ID = 2,
                Name = "Mountain Bike",
                Description = "Steel frame mountain bike with shocks!  Choose your size and color!",
                Image = "/images/island.jpg",
                Size = 39,
                Price = 459.99m

            });

            
        }
        
        public IActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Yacht y = _yachts.Single(x => x.ID == id.Value);
                return View(y);
            }

            return NotFound();
        }
        public IActionResult Index()
        {
            return View(_yachts);
        }
    }
}