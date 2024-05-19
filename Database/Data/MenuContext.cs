using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Database.Models;
using System;

namespace Database.Data
{
    public class MenuContext: DbContext
    {
        private readonly string _connectionString;
        public MenuContext(string connectionString="")
        {
            var settings = new Settings();
            if (connectionString != "")
                _connectionString = connectionString;
            else
                _connectionString = $"Server={settings.DbHost};Port={settings.DbPort};User={settings.DbUser};Password={settings.DbPassword};Database={settings.DbDatabase}";
        }

        public DbSet<Models.MenuItem> MenuItem { get; set; }
        public DbSet<Models.Order> Order { get; set; }
        public DbSet<Models.OrderItem> OrderItem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                _connectionString,
                new MySqlServerVersion(new Version(8, 0, 32))
            );
        }
    }
}
