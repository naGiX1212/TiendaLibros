using Microsoft.EntityFrameworkCore;
using TiendaLibros.Models;

namespace TiendaLibros.Data
{
    public class TiendaLibrosContext : DbContext
    {
        public TiendaLibrosContext(DbContextOptions<TiendaLibrosContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new { bc.BookId, bc.CategoryId });

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
    }
}
