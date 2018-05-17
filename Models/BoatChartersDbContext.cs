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
        public DbSet<Yacht> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }

    public class BoatChartesUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Cart
    {
        public Cart()
        {
            this.CartItems = new HashSet<CartItem>();
        }

        public int ID { get; set; }
        public Guid CookieIdentifier { get; set; }
        public DateTime LastModified { get; set; }
        public ICollection<CartItem> CartItems { get; set; }

    }

    public class CartItem
    {
        public int ID { get; set; }
        public Cart Cart { get; set; }
        public Yacht Yacht { get; set; }
        public DateTime Dates { get; set; }
    }
}
