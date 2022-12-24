using System;
using bookcatalogservice.Domain.BookAggregate;
using Microsoft.EntityFrameworkCore;

namespace bookcatalogservice.Infrastructure.Persistence
{
    public class BookContext : DbContext
    {

        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }


        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasOne<Genre>(x => x.Genre).WithMany(b => b.Books);
            modelBuilder.Entity<Book>().HasOne<Author>(x => x.Author).WithMany(b => b.Books);          
        }
    }
}

