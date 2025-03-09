using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TiendaLibros.Models;

namespace TiendaLibros.Data
{
    public class TiendaLibrosContext : IdentityDbContext<IdentityUser>
    {
        public TiendaLibrosContext(DbContextOptions<TiendaLibrosContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Llamamos a la configuración base de Identity
            base.OnModelCreating(modelBuilder);

            // Configuración de claves primarias para BookCategory
            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new { bc.BookId, bc.CategoryId });
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
    }
}