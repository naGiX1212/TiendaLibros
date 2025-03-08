using Microsoft.AspNetCore.Mvc;
using TiendaLibros.Data;

namespace TiendaLibros.Controllers
{
    public class CategoryController : Controller
    {
        private readonly TiendaLibrosContext _tiendaContext;
        public CategoryController(TiendaLibrosContext tiendaLibrosContext)
        {
            _tiendaContext = tiendaLibrosContext; ;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = _tiendaContext.Categories.ToList();

            return View(categories);
        }
        
    }
}
