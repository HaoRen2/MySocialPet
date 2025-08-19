using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Autenticacion
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El email de usuario es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

    }
}
