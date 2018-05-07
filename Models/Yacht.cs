using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyStore.Models
{
    public class Yacht   
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? PriceHighSeason { get; set; }
        public decimal? PriceOffSeason { get; set; }
        public string Image { get; set; }
        public int Size { get; set; }
        public int Year { get; set; }
        public int Cabins { get; set; }
        public bool AirCond { get; set; }


    }
}
