﻿// <auto-generated />
using System;
using Belcukerkka.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Belcukerkka.Repositories.Migrations
{
    [DbContext(typeof(CandyShopDbContext))]
    [Migration("20211018201239_DropCartAndCartItemsTables")]
    partial class DropCartAndCartItemsTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Belcukerkka.Models.Entities.Box", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoxParentId")
                        .HasColumnType("int");

                    b.Property<int?>("CompositionId")
                        .HasColumnType("int");

                    b.Property<double?>("FullPrice")
                        .HasColumnType("float");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BoxParentId");

                    b.HasIndex("CompositionId");

                    b.ToTable("Boxes");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.BoxPackage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("BoxPackages");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Картон"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Текстиль"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Дерево"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Туба"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Мягкая игрушка"
                        });
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.BoxParent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoxPackageId")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BoxPackageId");

                    b.ToTable("BoxParents");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.Candy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Candies");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.CandyInComposition", b =>
                {
                    b.Property<int>("CandyId")
                        .HasColumnType("int");

                    b.Property<int>("CompositionId")
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasKey("CandyId", "CompositionId");

                    b.HasIndex("CompositionId");

                    b.ToTable("CandiesInCompositions");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.CatalogItem", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChildBoxes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id", "Weight")
                        .IsUnique();

                    b.ToView("bel_vw_CatalogItems");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.Composition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.Property<int>("WeightTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WeightTypeId", "Weight")
                        .IsUnique();

                    b.ToTable("Compositions");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("CreatedBy")
                        .HasColumnType("tinyint");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int?>("InvoiceNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("BoxId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BoxId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.WeightType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WeightType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Классический"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Премиум"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Эксклюзивный"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Белорусский"
                        });
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.Box", b =>
                {
                    b.HasOne("Belcukerkka.Models.Entities.BoxParent", "BoxParent")
                        .WithMany("Boxes")
                        .HasForeignKey("BoxParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Belcukerkka.Models.Entities.Composition", "Composition")
                        .WithMany("Boxes")
                        .HasForeignKey("CompositionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("BoxParent");

                    b.Navigation("Composition");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.BoxParent", b =>
                {
                    b.HasOne("Belcukerkka.Models.Entities.BoxPackage", "BoxPackage")
                        .WithMany("BoxParents")
                        .HasForeignKey("BoxPackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BoxPackage");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.CandyInComposition", b =>
                {
                    b.HasOne("Belcukerkka.Models.Entities.Candy", "Candy")
                        .WithMany("CandyInCompositions")
                        .HasForeignKey("CandyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Belcukerkka.Models.Entities.Composition", "Composition")
                        .WithMany("CandiesInComposition")
                        .HasForeignKey("CompositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Candy");

                    b.Navigation("Composition");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.Composition", b =>
                {
                    b.HasOne("Belcukerkka.Models.Entities.WeightType", "WeightType")
                        .WithMany("Compositions")
                        .HasForeignKey("WeightTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WeightType");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.Order", b =>
                {
                    b.HasOne("Belcukerkka.Models.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.OrderItem", b =>
                {
                    b.HasOne("Belcukerkka.Models.Entities.Box", "Box")
                        .WithMany("OrderItems")
                        .HasForeignKey("BoxId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Belcukerkka.Models.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Box");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.Box", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.BoxPackage", b =>
                {
                    b.Navigation("BoxParents");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.BoxParent", b =>
                {
                    b.Navigation("Boxes");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.Candy", b =>
                {
                    b.Navigation("CandyInCompositions");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.Composition", b =>
                {
                    b.Navigation("Boxes");

                    b.Navigation("CandiesInComposition");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.Customer", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Belcukerkka.Models.Entities.WeightType", b =>
                {
                    b.Navigation("Compositions");
                });
#pragma warning restore 612, 618
        }
    }
}
