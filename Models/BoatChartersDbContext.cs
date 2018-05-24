using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

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
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
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
    public class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public int ID { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime LastModified { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public string Country { get; set; }
        public string PhoneNumber { get; set; }

        public string State { get; set; }
        public string Zip { get; set; }
        public string NameOnCard { get; set; }
        public string CCnumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string CVV { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

    }
    public class OrderItem
    {
        public int ID { get; set; }
        public Order Order { get; set; }
        public Yacht Yacht { get; set; }
        public DateTime Dates { get; set; }

    }
}
