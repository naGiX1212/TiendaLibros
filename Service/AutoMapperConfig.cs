using AutoMapper;
using TiendaLibros.Models;
using TiendaLibros.Models.ViewModels;

namespace TiendaLibros.Service
{
    public class AutoMapperConfig: Profile
    {

        public AutoMapperConfig()
        {
            CreateMap<Book, GetBook>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                     var bookCategories = context.Items["BookCategories"] as List<BookCategory>;
                      return bookCategories?
                     .Where(o => o.BookId == src.Id)
                     .Select(o => new CategoryViewModel { Name = o.Category.Name })
                     .ToList();
                }))
                 .ForMember(dest => dest.CoverImage, opt => opt.MapFrom((src, dest, destMember, context) =>
                 {
                     var imageService = context.Items["ImageService"] as IImageService;
                     return imageService != null ? imageService.ToBase64(src.CoverImage) : string.Empty;
                 }));
            CreateMap<CreateBookViewModel, Book>();

        }


    }
}
