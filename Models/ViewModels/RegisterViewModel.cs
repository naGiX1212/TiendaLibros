using System.ComponentModel.DataAnnotations;

namespace TiendaLibros.Models.ViewModels
{
    public class RegisterViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = " No hacen match...")]
        public string ConfirmedPassword { get; set; }

    }
}
