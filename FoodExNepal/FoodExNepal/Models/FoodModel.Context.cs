﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FoodExNepal.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FoodExEntities : DbContext
    {
        public FoodExEntities()
            : base("name=FoodExEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DeliveryStaff> DeliveryStaffs { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantFoodCategory> RestaurantFoodCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<WebsiteProfile> WebsiteProfiles { get; set; }
    }
}
