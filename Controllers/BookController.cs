using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.IO;
using TiendaLibros.Data;
using TiendaLibros.Data.UnitOfWork;
using TiendaLibros.Models;
using TiendaLibros.Models.ViewModels;
using TiendaLibros.Service;
using static System.Reflection.Metadata.BlobBuilder;

namespace TiendaLibros.Controllers
{
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IImageService _imageService;
        public BookController(IUnitOfWork unitOfWork,IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;

        }

        public IActionResult Index()
        {
            var books = _unitOfWork.Books.GetAll().ToList();
            var cat = _unitOfWork.BookCategories.GetAllWithIncludes(o => o.Category).ToList();
            var booksVM = new List<GetBook>();
            foreach (var book in books)
            {
                booksVM.Add(new GetBook
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Description = book.Description,
                    ISBN = book.ISBN,
                    Categories = new List<CategoryViewModel>()
                    {
                       cat.Where(o => o.BookId == book.Id).Select(o => new CategoryViewModel {Name = o.Category.Name }).FirstOrDefault()
                    },

                    CoverImage = _imageService.ToBase64(book.CoverImage)
                });
            }
               ;
            return View(booksVM);
        }

        [Authorize]

        public IActionResult CreateBook()
        {
            var bookVM = new CreateBookViewModel
            {
                AllCategories = _unitOfWork.Categories.GetAll().ToList()
            };
                
            return View(bookVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult CreateBook(CreateBookViewModel bookVM, IFormFile img)
        {

            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Title = bookVM.Title,
                    Author = bookVM.Author,
                    Description = bookVM.Description,
                    ISBN = bookVM.ISBN
                };
                if (img != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        
                        img.CopyTo(ms);

                        ms.Position = 0; // Reinicia el Stream
                        using (var image = Image.Load(ms))
                        {
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(500, 750),
                                Mode = ResizeMode.Crop
                            })); 


                               
                            // Guardar la imagen redimensionada
                            ms.SetLength(0);
                            image.Save(ms, new JpegEncoder());
                        }
                        book.CoverImage = ms.ToArray();

                    }
                }
                foreach (var categoryId in bookVM.SelectedCategories)  // Asumimos que Categories es una lista de IDs de categoría
                {
                    var category = _unitOfWork.Categories.GetById(categoryId);
                    if (category != null)
                    {
                        var bookCategory = new BookCategory
                        {
                            Book = book,
                            Category = category
                        };
                        _unitOfWork.BookCategories.Add(bookCategory);  // Agrega la relación a la base de datos
                    }
                    _unitOfWork.Books.Add(book);
                    
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");

            }
            return View(bookVM);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            var book = _unitOfWork.Books.GetById(id);
            if (book != null) {
                _unitOfWork.Books.Delete(book);
                _unitOfWork.Save();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var book = _unitOfWork.Books.GetById(id);
            if (book != null)
            {
                var cat = _unitOfWork.BookCategories.GetAllWithIncludes(o => o.Category).ToList();
                var booksVM = new GetBook()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    Description = book.Description,
                    ISBN = book.ISBN,
                    Categories = cat.Where(o => o.BookId == book.Id)
                                    .Select(o => new CategoryViewModel { Name = o.Category.Name })
                                    .ToList(),
                    CoverImage = _imageService.ToBase64(book.CoverImage)
                };

                return View(booksVM);
            }

            return NotFound();
        }
    }
}
