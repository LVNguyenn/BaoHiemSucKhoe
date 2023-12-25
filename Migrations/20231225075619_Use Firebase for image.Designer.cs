﻿// <auto-generated />
using System;
using InsuranceManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InsuranceManagement.Migrations
{
    [DbContext(typeof(UserDbContext))]
    [Migration("20231225075619_Use Firebase for image")]
    partial class UseFirebaseforimage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InsuranceManagement.Domain.Insurance", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("period")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("price")
                        .HasColumnType("int");

                    b.Property<string>("title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("insurances");
                });

            modelBuilder.Entity("InsuranceManagement.Domain.Purchase", b =>
                {
                    b.Property<Guid>("id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("userID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("purchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id", "userID");

                    b.HasIndex("userID");

                    b.ToTable("purchases");
                });

            modelBuilder.Entity("InsuranceManagement.Domain.User", b =>
                {
                    b.Property<Guid>("userID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("displayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("userID");

                    b.ToTable("users");
                });

            modelBuilder.Entity("InsuranceManagement.Domain.Purchase", b =>
                {
                    b.HasOne("InsuranceManagement.Domain.Insurance", "Insurance")
                        .WithMany("Purchases")
                        .HasForeignKey("id")
                        .HasConstraintName("FK_Purchase_Insurance")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InsuranceManagement.Domain.User", "User")
                        .WithMany("Purchases")
                        .HasForeignKey("userID")
                        .HasConstraintName("FK_Purchase_User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Insurance");

                    b.Navigation("User");
                });

            modelBuilder.Entity("InsuranceManagement.Domain.Insurance", b =>
                {
                    b.Navigation("Purchases");
                });

            modelBuilder.Entity("InsuranceManagement.Domain.User", b =>
                {
                    b.Navigation("Purchases");
                });
#pragma warning restore 612, 618
        }
    }
}
