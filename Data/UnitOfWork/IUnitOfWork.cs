using TiendaLibros.Data.Repository;
using TiendaLibros.Models;

namespace TiendaLibros.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Book> Books { get; set; }
        IRepository<Category> Categories { get; set; }
        IRepository<BookCategory> BookCategories { get; set; }

        void Save();
    }
}
