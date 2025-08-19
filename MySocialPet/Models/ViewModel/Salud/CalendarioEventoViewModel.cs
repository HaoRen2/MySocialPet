using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Salud
{

    public class CalendarioEventosViewModel
    {
        public int IdMascota { get; set; }
        public string NombreMascota { get; set; }
        public string Especie { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public List<EventoViewModel>? Eventos { get; set; }
    }

    public class EventoViewModel
    {
        public int? IdEvento { get; set; }
        public int IdMascota { get; set; }
        [Required(ErrorMessage = "El campo Titulo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El máximo permitido es de 50 caracteres.")]
        public string Titulo { get; set; }
        public DateTime FechaHora { get; set; }
        public string Color { get; set; }
        [Required(ErrorMessage = "El campo Notas es obligatorio.")]
        [StringLength(100, ErrorMessage = "El máximo permitido es de 100 caracteres.")]
        public string Notas { get; set; }
    }

}
