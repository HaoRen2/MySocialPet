using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Mascotas
{
    public class CrearMascotaViewModel
    {
        [Required]
        public string Nombre { get; set; }
        public DateTime? Nacimiento { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Solo números positivos")]
        public decimal PesoKg { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Solo números positivos")]
        public decimal LongitudCm { get; set; }
        [Required]
        public string Genero { get; set; }

        public IFormFile? Foto { get; set; }

        public int BCS { get; set; }
        public bool Esterilizada { get; set; }
        [Required]
        public int IdRaza { get; set; }

        public IEnumerable<SelectListItem>? Especies { get; set; }
        public IEnumerable<SelectListItem>? Razas { get; set; }
    }
}
