using System.ComponentModel.DataAnnotations;

namespace Practica.TiendasAPI.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas deben coincidir.")]
        public string ConfirmPassword { get; set; }

    }

}
