using System;
using reviewservice.Domain.ReviewAggregate;
using Microsoft.EntityFrameworkCore;

namespace reviewservice.Infrastructure.Persistence
{
    public class ReviewContext : DbContext
    {
        public DbSet<Review> Reviews { get; set; }       

        public ReviewContext(DbContextOptions<ReviewContext> options) : base(options)
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

