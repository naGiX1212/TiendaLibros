namespace TiendaLibros.Models.ViewModels
{
    public class GetBook
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public string CoverImage { get; set; }
    }
}
