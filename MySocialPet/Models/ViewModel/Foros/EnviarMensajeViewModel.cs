using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Foros
{
    public class EnviarMensajeViewModel
    {
        [Required]
        public int IdForo { get; set; }

        [Required]
        public int IdDiscusion { get; set; }

        public int? IdMensajePadre { get; set; } // null si es nuevo mensaje

        [Required(ErrorMessage = "El contenido del mensaje es obligatorio.")]
        [StringLength(2000, ErrorMessage = "El mensaje no puede superar los 2000 caracteres.")]
        public string Contenido { get; set; }
    }
}
