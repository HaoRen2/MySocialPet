using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Mascotas
{
    public class ListaMascotaViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime? Nacimiento { get; set; }

        [Display(Name = "Peso (kg)")]
        public decimal? PesoKg { get; set; }

        [Display(Name = "Longitud (cm)")]
        public decimal? LongitudCm { get; set; }

        [StringLength(10)]
        public string Genero { get; set; }

        public byte[] Foto { get; set; }

        [Display(Name = "BCS")]
        public decimal? BCS { get; set; }

        [Display(Name = "Esterilizado/a")]
        public bool Esterilizada { get; set; }

        public int IdRaza { get; set; }

        [Display(Name = "Raza")]
        public string NombreRaza { get; set; }
    }
}
