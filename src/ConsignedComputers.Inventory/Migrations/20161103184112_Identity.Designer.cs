using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ConsignedComputers.Inventory.Models;

namespace ConsignedComputers.Inventory.Migrations
{
    [DbContext(typeof(InventoryContext))]
    [Migration("20161103184112_Identity")]
    partial class Identity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("ConsignedComputers.Inventory.Models.ProductInventory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 1000);

                    b.Property<int>("InStock");

                    b.Property<DateTime>("LastUpdate");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<decimal>("PriceUSD");

                    b.HasKey("ID");

                    b.ToTable("Products");
                });
        }
    }
}
