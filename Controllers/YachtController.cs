using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyStore.Models;
using Microsoft.Extensions.Configuration;

namespace MyStore.Controllers
{
    public class YachtController : Controller
    { 
        
        private string _BoatsConnectionString = null;

        public YachtController(IConfiguration config)
        {
            //using Microsoft.Extensions.Configuration
            _BoatsConnectionString = config.GetConnectionString("Boats");
        }
           
        public IActionResult Index()
        {
            //For now, I'm using a List to mock up my product data            
             List<Yacht> _yachts = new List<Yacht>();
                                                            
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(_BoatsConnectionString))
            {
                connection.Open();

                using (System.Data.SqlClient.SqlCommand command = connection.CreateCommand())
                {
                    //I usually write a query that I want in the Query window, and copy it here when I'm happy with how it works...
                    //Realistically, I should make my query a stored procedure and call that instead.  It'll run faster that way.
                    command.CommandText = "sp_GetAllBoats";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader())
                    {
                        int idColumn = reader.GetOrdinal("ID");
                        int nameColumn = reader.GetOrdinal("BoatName");
                        int descriptionColumn = reader.GetOrdinal("Descriptions");
                        int priceHighSeasonColumn = reader.GetOrdinal("PriceHighSeason");
                        int priceOffSeasonColumn = reader.GetOrdinal("PriceOffSeason");
                        int imagePathColumn = reader.GetOrdinal("ImagePath");
                        int sizeColumn = reader.GetOrdinal("Size");
                        int boatYearColumn = reader.GetOrdinal("BoatYear");
                        int cabinsColumn = reader.GetOrdinal("Cabins");
                        int airCondColumn = reader.GetOrdinal("AirCond");


                        while (reader.Read())
                        {
                            int id = reader.GetInt32(idColumn);
                            string boatName = reader.GetString(nameColumn);
                            string descriptions = reader.GetString(descriptionColumn);
                            decimal priceHighSeason = reader.GetDecimal(priceHighSeasonColumn);
                            decimal priceOffSeason = reader.GetDecimal(priceOffSeasonColumn);
                            string imagePath = reader.GetString(imagePathColumn);
                            int size = reader.GetInt32(sizeColumn);
                            int boatYear = reader.GetInt32(boatYearColumn);
                            int cabins = reader.GetInt32(cabinsColumn);
                            bool airCond = reader.GetBoolean(airCondColumn);

                            _yachts.Add(new Yacht
                            {
                                ID = id,
                                Name = boatName,
                                Description = descriptions,
                                PriceHighSeason = priceHighSeason,
                                PriceOffSeason = priceOffSeason,
                                Image = imagePath,
                                Size = size,
                                Year = boatYear,
                                Cabins = cabins,
                                AirCond = airCond


                            });
                        }
                    }
                }
                
                
            }
            //By passing a parameter to my View method, I'm passing it to the CSHTML so it can be "bound" up to the View.
            return View(_yachts);
        }
        public IActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                Yacht p = null;


                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(_BoatsConnectionString))
                {
                    connection.Open();

                    System.Data.SqlClient.SqlCommand command = connection.CreateCommand();

                    //I usually write a query that I want in the Query window, and copy it here when I'm happy with how it works...
                    //Realistically, I should make my query a stored procedure and call that instead.  It'll run faster that way.
                    command.CommandText = "sp_GetProduct";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", id.Value);

                    using (System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader())
                    {


                        while (reader.Read())
                        {
                            p = new Yacht
                            {
                                ID = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2),
                                 PriceHighSeason = reader.GetDecimal(3),
                                PriceOffSeason = reader.GetDecimal(4),
                                Image = reader.GetString(5),
                                Size = reader.GetInt32(6),
                                Year = reader.GetInt32(7),
                                Cabins = reader.GetInt32(8),
                                AirCond = reader.GetBoolean(9),
                            };

                        }
                    }
                   
                    
                }
                if (p != null)
                {
                    return View(p);
                }
            }
            return NotFound();
        }
    }
}