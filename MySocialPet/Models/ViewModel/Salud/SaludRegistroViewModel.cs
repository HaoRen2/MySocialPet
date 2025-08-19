using MySocialPet.Models.Salud;
using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Salud
{
    public class SaludRegistroViewModel
    {
        public int IdMascota { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "El peso es obligatorio.")]
        [Range(0, 500, ErrorMessage = "Solo números positivos")]
        [Display(Name = "Peso (kg)")]
        public decimal PesoKg { get; set; }

        [Required(ErrorMessage = "El peso es obligatorio.")]
        [Range(0, 500, ErrorMessage = "Solo números positivos")]
        [Display(Name = "Longitud Lomo (cm)")]
        public decimal LongitudCm { get; set; }

        public int? BCS { get; set; }
        public string? BcsTexto { get; set; }

        public string? NombreMascota { get; set; }

    }
}
