using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Autenticacion
{
    public class RecuperarContrasenyaViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Email { get; set; }
    }
}
