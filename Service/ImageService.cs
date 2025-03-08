namespace TiendaLibros.Service
{
    public class ImageService : IImageService
    {
        public string ToBase64(byte[] image)
        {
            return Convert.ToBase64String(image);
        }
    }
}
