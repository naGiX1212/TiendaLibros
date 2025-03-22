using System.ComponentModel.DataAnnotations;

namespace TiendaLibros.Models
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Author { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public List<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
        [Required]

        public string ISBN { get; set; } = string.Empty;
        [Required]
        public byte[]? CoverImage { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

    }
}
