using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TiendaLibros.Models
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        public List<BookCategory> BookCategories { get; set; } = new List<BookCategory>();


    }
}