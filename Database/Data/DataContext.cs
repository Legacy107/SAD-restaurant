using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Database.Models;
using System;

namespace Database.Data
{
    public class DataContext: DbContext
    {
        private readonly string _connectionString;
        public DataContext(string connectionString="")
        {
            var settings = new Settings();
            if (connectionString != "")
                _connectionString = connectionString;
            else
                _connectionString = $"Server={settings.DbHost};Port={settings.DbPort};User={settings.DbUser};Password={settings.DbPassword};Database={settings.DbDatabase}";
        }

        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<MenuItemCategory> MenuItemCategory { get; set; }
        public DbSet<MenuItemOption> MenuItemOption { get; set; }
        public DbSet<MenuItemVariation> MenuItemVariation { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceItem> InvoiceItem { get; set; }
        public DbSet<Receipt> Receipt { get; set; }

        public DbSet<Models.Table> Tables { get; set; }
        public DbSet<Models.Reservation> Reservations { get; set; }
        public DbSet<Models.CheckIn> CheckIns { get; set; }
        public DbSet<Models.TableCheckIn> TableCheckIn { get; set; }
        public DbSet<Models.Order> Order { get; set; }
        public DbSet<Models.OrderItem> OrderItem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                _connectionString,
                new MySqlServerVersion(new Version(8, 0, 32))
            );
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TableReservation>(builder =>
            {
                builder.HasKey(tableReservation => new { tableReservation.TableId, tableReservation.ReservationId });
            });

            builder.Entity<TableReservation>()
                .HasOne(tr => tr.Table)
                .WithMany(table => table.Reservations)
                .HasForeignKey(tr => tr.TableId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TableReservation>()
                .HasOne(tr => tr.Reservation)
                .WithMany(reservation => reservation.Tables)
                .HasForeignKey(tr => tr.ReservationId);

            builder.Entity<TableCheckIn>(builder =>
            {
                builder.HasKey(tableCheckIn => new { tableCheckIn.TableId, tableCheckIn.CheckInId });
            });

            builder.Entity<TableCheckIn>()
                .HasOne(tc => tc.Table)
                .WithMany(table => table.CheckIns)
                .HasForeignKey(tc => tc.TableId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TableCheckIn>()
                .HasOne(tc => tc.CheckIn)
                .WithMany(checkin => checkin.Tables)
                .HasForeignKey(tc => tc.CheckInId);
        }
    }
}
