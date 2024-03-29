﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace wpd2coursework.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        /// <summary>&lt;DbSet&gt; of type Product Used to create instance of the Products Table</summary>
        public DbSet<Product> Products { get; set; }
        /// <summary>&lt;DbSet&gt; of type Category Used to create instance of the Categories Table</summary>
        public DbSet<Category> Categories { get; set; }
        /// <summary>&lt;DbSet&gt; of type Customer Used to create instance of the CustomerOrders Table</summary>
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        /// <summary>&lt;DbSet&gt; of type OrderedProduct Used to create instance of the OrderedProducts Table</summary>
        public DbSet<OrderedProduct> Orderedproducts { get; set; }
        /// <summary>&lt;DbSet&gt; of type Cart Used to create instance of the Carts Table</summary>
        public DbSet<Cart> Carts { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}