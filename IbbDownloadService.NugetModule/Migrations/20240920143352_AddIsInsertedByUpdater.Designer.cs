﻿// <auto-generated />
using System;
using IbbDownloadService.NugetModule.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IbbDownloadService.NugetModule.Migrations
{
    [DbContext(typeof(NugetDbContext))]
    [Migration("20240920143352_AddIsInsertedByUpdater")]
    partial class AddIsInsertedByUpdater
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Nugets")
                .HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("IbbDownloadService.NugetModule.Entities.Nuget", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsInsertedByUpdater")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Md5")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("NeedsVerification")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("VerifiedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Nugets", "Nugets");
                });
#pragma warning restore 612, 618
        }
    }
}
