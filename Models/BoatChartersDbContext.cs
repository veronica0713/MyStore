using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyStore.Models
{
    public class BoatChartersDbContext : IdentityDbContext<BoatChartesUser>
    {
        public BoatChartersDbContext() : base()
        {

        }

        public BoatChartersDbContext(DbContextOptions options) : base(options)
        {

        }
    }

    public class BoatChartesUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
