using Microsoft.EntityFrameworkCore;
using TiendaLibros.Data.Repository;
using TiendaLibros.Models;

namespace TiendaLibros.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TiendaLibrosContext _context;

        public IRepository<Book> Books { get;  set; }
        public IRepository<Category> Categories { get;  set; }
        public IRepository<BookCategory> BookCategories { get;  set; }

        public UnitOfWork(TiendaLibrosContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            Books = new Repository<Book>(_context);
            Categories = new Repository<Category>(_context);
            BookCategories = new Repository<BookCategory>(_context);
        }

        public void Save()
        {
            _context.SaveChanges(); // Llama al DbContext para guardar los cambios
        }
    }

}
