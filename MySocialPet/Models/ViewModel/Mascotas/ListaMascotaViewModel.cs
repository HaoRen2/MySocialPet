using Microsoft.AspNetCore.Mvc.Rendering;
using MySocialPet.Models.Salud;
using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Mascotas
{
    public class ListaMascotaViewModel
    {
        public int? IdMascota { get; set; }

        public string Nombre { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime? Nacimiento { get; set; }

        [Display(Name = "Peso (kg)")]
        public decimal? PesoKg { get; set; }

        [Display(Name = "Longitud Lomo (cm)")]
        public decimal? LongitudCm { get; set; }

        [StringLength(10)]
        public string Genero { get; set; }

        public byte[] Foto { get; set; }

        [Display(Name = "Indice Corporal")]
        public int? BCS { get; set; }

        [Display(Name = "Esterilizado/a")]
        public bool Esterilizada { get; set; }

        public int IdRaza { get; set; }

        [Display(Name = "Raza")]
        public string NombreRaza { get; set; }

        public Evento? Evento { get; set; }

        public string? RazaFoto { get; set; }


        public int? PaginaActual { get; set; }
        public int? TotalPaginas { get; set; }

        public bool? TienePaginaAnterior => PaginaActual > 1;
        public bool? TienePaginaSiguiente => PaginaActual < TotalPaginas;
    }
}
