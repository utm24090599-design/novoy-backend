using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace backend.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Bus> Buses { get; set; }
        //public DbSet<BusStopGn> BusStopGns { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("person");
            modelBuilder.Entity<Price>().ToTable("price");
            modelBuilder.Entity<Bus>().ToTable("bus");
        }

    }
}
