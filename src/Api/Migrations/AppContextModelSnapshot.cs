using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Api;

namespace Api.Migrations
{
    [DbContext(typeof(AppContext))]
    partial class AppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Api.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("NumberOfDiners");

                    b.Property<string>("OrganiserForename")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("OrganiserSurname")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("OrganiserTelephoneNumber")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("Api.Models.Identity.AspNetRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Api.Models.Identity.AspNetRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AspNetRoleId");

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("AspNetRoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Api.Models.Identity.AspNetUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(450);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("AspNetRoleId");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("AspNetRoleId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Api.Models.Identity.AspNetUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AspNetUserId");

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("AspNetUserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Api.Models.Identity.AspNetUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(450);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(450);

                    b.Property<string>("AspNetUserId");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450);

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("AspNetUserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Api.Models.Identity.AspNetUserRole", b =>
                {
                    b.Property<int>("AspNetUserId");

                    b.Property<int>("RoleId");

                    b.Property<int?>("RoleAspNetUserId");

                    b.Property<int?>("RoleId1");

                    b.Property<string>("UserId");

                    b.HasKey("AspNetUserId", "RoleId");

                    b.HasIndex("UserId");

                    b.HasIndex("RoleAspNetUserId", "RoleId1");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Api.Models.Identity.AspNetUserToken", b =>
                {
                    b.Property<string>("UserId")
                        .HasMaxLength(450);

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(450);

                    b.Property<string>("Name")
                        .HasMaxLength(450);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Api.Models.Identity.AspNetRoleClaim", b =>
                {
                    b.HasOne("Api.Models.Identity.AspNetRole", "AspNetRole")
                        .WithMany("AspNetRoleClaims")
                        .HasForeignKey("AspNetRoleId");
                });

            modelBuilder.Entity("Api.Models.Identity.AspNetUser", b =>
                {
                    b.HasOne("Api.Models.Identity.AspNetRole")
                        .WithMany("AspNetUsers")
                        .HasForeignKey("AspNetRoleId");
                });

            modelBuilder.Entity("Api.Models.Identity.AspNetUserClaim", b =>
                {
                    b.HasOne("Api.Models.Identity.AspNetUser", "AspNetUser")
                        .WithMany("AspNetUserClaims")
                        .HasForeignKey("AspNetUserId");
                });

            modelBuilder.Entity("Api.Models.Identity.AspNetUserLogin", b =>
                {
                    b.HasOne("Api.Models.Identity.AspNetUser", "AspNetUser")
                        .WithMany("AspNetUserLogins")
                        .HasForeignKey("AspNetUserId");
                });

            modelBuilder.Entity("Api.Models.Identity.AspNetUserRole", b =>
                {
                    b.HasOne("Api.Models.Identity.AspNetUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.HasOne("Api.Models.Identity.AspNetUserRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleAspNetUserId", "RoleId1");
                });
        }
    }
}
