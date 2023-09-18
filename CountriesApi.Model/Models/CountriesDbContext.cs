using System;
using Microsoft.EntityFrameworkCore;

namespace CountriesApi.Model.Models
{
	public class CountriesDbContext : DbContext
	{
		public DbSet <Country> Countries { get; set; }
		public DbSet <City> Cities  { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=127.0.0.1;Port=3306;Database=world;User=root;Password=LihiYohai021221;";
            optionsBuilder.UseMySQL(connectionString);
        }
    }
}

