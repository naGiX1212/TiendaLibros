
namespace TiendaLibros.Models
{
    public class BookCategory
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }  // Relación de navegación con Book

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }  // Relación de navegación con Category
    }

}
