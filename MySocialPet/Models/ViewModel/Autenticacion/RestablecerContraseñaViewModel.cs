using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Autenticacion
{
    public class RestablecerContrasenyaViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(3)]
        [Display(Name = "Nueva contraseña")]
        public string NuevaContrasenya { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NuevaContrasenya", ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmarContrasenya { get; set; }
    }
}
