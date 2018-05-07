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
                Name = "Lagoon 52 F (2017)",
                Image = "/images/cat1.jpg",
                Description = "The Lagoon 52 F is the latest model developed by the world’s leading builder of cruising catamarans and marks a whole new era in the design of such boats.",
                Year = 2017,
                Size = 52,
                PriceHighSeason = 16000m,
                PriceOffSeason = 13000m,
                Cabins = 4,
                AirCond = true


            });
            _yachts.Add(new Yacht
            {
                ID = 2,
                Name = "Lagoon 50 (2019)",
                Image = "/images/cat-details2.jpg",
                Description = "Our brand new Lagoon 50 (2019) called “Lilamar” is ideal for large groups as she offers 5 double cabins and 5 electric heads. She will be available as from February 2019 in the BVI Fleet.",
                Year = 2019,
                Size = 50,
                PriceHighSeason = 16000m,
                PriceOffSeason = 13000m,
                Cabins = 5,
                AirCond = true

            });
            _yachts.Add(new Yacht
            {
                ID = 3,
                Name = " FP Saba 50 (2017)",
                Image = "/images/cat-details-3.jpg",
                Description = "Our brand new Saba 50, Liberty,  is light, spacious and comfortable.  She offers you all the pleasures of the sea, both under sail and at anchor where relaxation goes hand in hand with comfort and pleasure.",
                Year = 2017,
                Size = 50,
                PriceHighSeason = 15700m,
                PriceOffSeason = 12700m,
                Cabins = 5,
                AirCond = true

            });
            _yachts.Add(new Yacht
            {
                ID = 4,
                Name = " Lagoon 450 F (2017)",
                Image = "/images/cat-details1.jpg",
                Description = " She is the owner version with a very spacious master cabin and master bathroom in one hull and 2 additional queen cabins in the other hull. ",
                Year = 2017,
                Size = 45,
                PriceHighSeason = 10000m,
                PriceOffSeason = 7000m,
                Cabins = 3,
                AirCond = false

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