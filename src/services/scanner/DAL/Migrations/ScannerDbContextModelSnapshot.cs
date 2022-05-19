﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(ScannerDbContext))]
    partial class ScannerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-preview.4.22229.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entity.Garbage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Picture")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Barcode")
                        .IsUnique();

                    b.ToTable("Garbage");
                });

            modelBuilder.Entity("DAL.Entity.GarbageReceptionPoint", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Latitude")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Longitude")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("GarbageReceptionPoint");
                });

            modelBuilder.Entity("DAL.Entity.GarbageType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("GarbageType");
                });

            modelBuilder.Entity("DAL.Entity.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("DAL.Entity.UserGarbage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("GarbageId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ScannedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GarbageId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGarbage");
                });

            modelBuilder.Entity("DAL.Entity.UserGarbageFromApi", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserGarbageFromApi");
                });

            modelBuilder.Entity("GarbageGarbageType", b =>
                {
                    b.Property<long>("GarbageTypesId")
                        .HasColumnType("bigint");

                    b.Property<long>("GarbagesId")
                        .HasColumnType("bigint");

                    b.HasKey("GarbageTypesId", "GarbagesId");

                    b.HasIndex("GarbagesId");

                    b.ToTable("GarbageGarbageType");
                });

            modelBuilder.Entity("GarbageReceptionPointGarbageType", b =>
                {
                    b.Property<long>("GarbageReceptionPointsId")
                        .HasColumnType("bigint");

                    b.Property<long>("GarbageTypesId")
                        .HasColumnType("bigint");

                    b.HasKey("GarbageReceptionPointsId", "GarbageTypesId");

                    b.HasIndex("GarbageTypesId");

                    b.ToTable("GarbageReceptionPointGarbageType");
                });

            modelBuilder.Entity("DAL.Entity.UserGarbage", b =>
                {
                    b.HasOne("DAL.Entity.Garbage", "Garbage")
                        .WithMany()
                        .HasForeignKey("GarbageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Garbage");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DAL.Entity.UserGarbageFromApi", b =>
                {
                    b.HasOne("DAL.Entity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GarbageGarbageType", b =>
                {
                    b.HasOne("DAL.Entity.GarbageType", null)
                        .WithMany()
                        .HasForeignKey("GarbageTypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entity.Garbage", null)
                        .WithMany()
                        .HasForeignKey("GarbagesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GarbageReceptionPointGarbageType", b =>
                {
                    b.HasOne("DAL.Entity.GarbageReceptionPoint", null)
                        .WithMany()
                        .HasForeignKey("GarbageReceptionPointsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entity.GarbageType", null)
                        .WithMany()
                        .HasForeignKey("GarbageTypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
