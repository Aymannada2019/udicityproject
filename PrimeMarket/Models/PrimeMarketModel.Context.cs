﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace PrimeMarket.Models
{

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class PrimeMarketEntities : DbContext
{
    public PrimeMarketEntities()
        : base("name=PrimeMarketEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartStatu> CartStatus { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Commodity> Commodities { get; set; }

    public virtual DbSet<CommodityImage> CommodityImages { get; set; }

    public virtual DbSet<CommodityRating> CommodityRatings { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Governorate> Governorates { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderStatu> OrderStatus { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<PriceUnit> PriceUnits { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<UserRating> UserRatings { get; set; }

    public virtual DbSet<Village> Villages { get; set; }

}

}

