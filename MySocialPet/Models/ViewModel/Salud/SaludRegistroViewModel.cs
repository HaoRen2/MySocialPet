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

        [Required]
        [Display(Name = "Peso (kg)")]
        public decimal PesoKg { get; set; }

        [Required]
        [Display(Name = "Longitud (cm)")]
        public decimal LongitudCm { get; set; }

        public int? BCS { get; set; }
        public string? BcsTexto { get; set; }

        public string? NombreMascota { get; set; }

    }
}
