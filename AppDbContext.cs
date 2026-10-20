using Microsoft.EntityFrameworkCore;
using RealEstate.ConsoleApp.Models;
using System.Collections.Generic;

namespace RealEstate.ConsoleApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { 

        }

        public DbSet<Property> Properties { get; set; } = null!;
    }
}