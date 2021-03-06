﻿using Api.Models;
using Api.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Api
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options)
            : base(options)
        { }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Diner> Diner { get; set; }
        public DbSet<DinerMenuItem> DinerMenuItems { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuSection> MenuSections { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }

        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUserLogin>()
                .HasKey(c => new { c.LoginProvider, c.ProviderKey });
            modelBuilder.Entity<AspNetUserToken>()
                .HasKey(c => new { c.UserId, c.LoginProvider, c.Name });
            modelBuilder.Entity<AspNetUserRole>()
                .HasKey(c => new { c.UserId, c.RoleId });
            modelBuilder.Entity<DinerMenuItem>()
                .HasKey(c => new { c.DinerId, c.MenuItemId });
            modelBuilder.Entity(typeof(Booking))
            .HasOne(typeof(AspNetUser), "LastUpdatedBy")
            .WithMany()
            .HasForeignKey("LastUpdatedById")
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity(typeof(Booking))
            .HasOne(typeof(Menu), "Menu")
            .WithMany()
            .HasForeignKey("MenuId")
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity(typeof(Booking))
            .HasOne(typeof(AspNetUser), "CreatedBy")
            .WithMany()
            .HasForeignKey("CreatedById")
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
