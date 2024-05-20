﻿// <auto-generated />
using System;
using Database.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class MenuContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Database.Models.CheckIn", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CheckInStart")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("CheckIns");
                });

            modelBuilder.Entity("Database.Models.MenuItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<float>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("MenuItem");
                });

            modelBuilder.Entity("Database.Models.Reservation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CName")
                        .HasColumnType("longtext");

                    b.Property<bool>("HasShownUp")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("ReservationStart")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Database.Models.Table", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Traits")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("Database.Models.TableCheckIn", b =>
                {
                    b.Property<int>("TableId")
                        .HasColumnType("int");

                    b.Property<Guid>("CheckInId")
                        .HasColumnType("char(36)");

                    b.HasKey("TableId", "CheckInId");

                    b.HasIndex("CheckInId");

                    b.ToTable("TableCheckIn");
                });

            modelBuilder.Entity("Database.Models.TableReservation", b =>
                {
                    b.Property<int>("TableId")
                        .HasColumnType("int");

                    b.Property<Guid>("ReservationId")
                        .HasColumnType("char(36)");

                    b.HasKey("TableId", "ReservationId");

                    b.HasIndex("ReservationId");

                    b.ToTable("TableReservation");
                });

            modelBuilder.Entity("Database.Models.TableCheckIn", b =>
                {
                    b.HasOne("Database.Models.CheckIn", "CheckIn")
                        .WithMany("Tables")
                        .HasForeignKey("CheckInId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Models.Table", "Table")
                        .WithMany("CheckIns")
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CheckIn");

                    b.Navigation("Table");
                });

            modelBuilder.Entity("Database.Models.TableReservation", b =>
                {
                    b.HasOne("Database.Models.Reservation", "Reservation")
                        .WithMany("Tables")
                        .HasForeignKey("ReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Models.Table", "Table")
                        .WithMany("Reservations")
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reservation");

                    b.Navigation("Table");
                });

            modelBuilder.Entity("Database.Models.CheckIn", b =>
                {
                    b.Navigation("Tables");
                });

            modelBuilder.Entity("Database.Models.Reservation", b =>
                {
                    b.Navigation("Tables");
                });

            modelBuilder.Entity("Database.Models.Table", b =>
                {
                    b.Navigation("CheckIns");

                    b.Navigation("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}
