using System;
using MyStore.Models;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;

namespace MyStore
{
    internal   class DbInitializer
    {
        internal static void Initialize(BoatChartersDbContext db)
        {
            db.Database.Migrate();

            if (db.Products.Count() == 0)
            {
                db.Products.Add(new Yacht
                {
                    Name = "Helia 450",
                    Description = "bla bla",
                    Image = "/images/cat1.jpg",
                    Size = 45,
                    PriceHighSeason = 11000m,
                    PriceOffSeason = 5000m,
                    Year = 2015,
                    AirCond = false,
                    Cabins = 4
                });
                db.Products.Add(new Yacht
                {
                    Name = "Helia 400",
                    Description = "bla bla bla",
                    Image = "/images/cat2.jpg",
                    Size = 40,
                    PriceHighSeason = 9000m,
                    PriceOffSeason = 4000m,
                    Year = 2015,
                    AirCond = false,
                    Cabins = 3
                });
                db.SaveChanges();
            }
        }
    }
}