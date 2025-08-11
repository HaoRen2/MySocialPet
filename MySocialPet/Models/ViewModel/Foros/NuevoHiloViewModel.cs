using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Foros
{
    public class NuevoHiloViewModel
    {
        [Required]
        public int IdForo { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(255, ErrorMessage = "El título no puede superar los 255 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }
    }
}