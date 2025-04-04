using AutoMapper;
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
        private readonly IMapper _mapper;

        private readonly IImageService _imageService;
        public BookController(IUnitOfWork unitOfWork,IImageService imageService,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _mapper = mapper;

        }

        public  IActionResult Index(int pageNumber = 1, int pageSize = 3)
        {
            var books = _unitOfWork.Books.GetAll().OrderBy(o => o.CreatedTime).ToList();
            var bookCategories = _unitOfWork.BookCategories.GetAllWithIncludes(o => o.Category).ToList();

            var booksVM = _mapper.Map<List<GetBook>>(books, opt =>
            {
                opt.Items["BookCategories"] = bookCategories;
                opt.Items["ImageService"] = _imageService;
            });

            var pagedList = PagedList<GetBook>.Create(booksVM, pageNumber, pageSize);
            return View(pagedList);
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
                var book = _mapper.Map<Book>(bookVM);
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
                var bookCategorie = _unitOfWork.BookCategories.GetAllWithIncludes(o => o.Category).ToList();
                var booksVM = _mapper.Map<GetBook>(book, opt =>
                {
                    opt.Items["BookCategories"] = bookCategorie;
                    opt.Items["ImageService"] = _imageService;
                });

                return View(booksVM);
            }

            return NotFound();
        }
    }
}
