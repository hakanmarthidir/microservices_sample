using System;
using shelveservice.Domain.ShelveAggregate;
using Microsoft.EntityFrameworkCore;

namespace shelveservice.Infrastructure.Persistence
{
    public class ShelveContext : DbContext
    {
        public DbSet<Shelve> Shelves { get; set; }       

        public ShelveContext(DbContextOptions<ShelveContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                  
        }
    }
}

