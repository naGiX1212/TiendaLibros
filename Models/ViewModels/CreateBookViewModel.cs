using System.ComponentModel.DataAnnotations;

namespace TiendaLibros.Models.ViewModels
{
    public class CreateBookViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Author { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        
        public List<Guid> SelectedCategories { get; set; } = new List<Guid>();
        [Required]
        public string ISBN { get; set; } = string.Empty;

        public List<Category> AllCategories { get; set; } = new List<Category>();
    }
}
