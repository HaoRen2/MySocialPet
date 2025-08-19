using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Mascotas
{
    public class CrearMascotaViewModel
    {

        public int? Id { get; set; }
        [Required(ErrorMessage = "El nombre obligatorio.")]
        public string Nombre { get; set; }
        public DateTime? Nacimiento { get; set; }

        [Required(ErrorMessage = "El peso es obligatorio.")]
        [Range(0, 500, ErrorMessage = "Solo números positivos")]
        public decimal PesoKg { get; set; }

        [Required(ErrorMessage = "La longitud es obligatorio.")]

        [Range(0, 500, ErrorMessage = "Solo números positivos")]
        public decimal LongitudCm { get; set; }

        [Required(ErrorMessage = "El genero es obligatorio.")]
        public string Genero { get; set; }

        public IFormFile? Foto { get; set; }

        public int? BCS { get; set; }
        public bool Esterilizada { get; set; }

        [Required(ErrorMessage = "La raza es obligatorio.")]
        public int IdRaza { get; set; }
        [Required]
        public int? IdEspecie { get; set; }

        public IEnumerable<SelectListItem>? Especies { get; set; }
        public IEnumerable<SelectListItem>? Razas { get; set; }
    }
}
