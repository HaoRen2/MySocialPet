using Microsoft.AspNetCore.Mvc.Rendering;
using MySocialPet.Models.Mascotas;
using MySocialPet.Models.Sugerencias;
using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Sugerencias
{
    public class SugerenciaViewModel
    {
        public IEnumerable<SelectListItem> EspeciesSelectList { get; set; }
        public IEnumerable<SelectListItem> CategoriasSelectList { get; set; }
        public IEnumerable<SelectListItem> RazasSelectList { get; set; }

        public int? IdEspecie { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdRaza { get; set; }

        public IEnumerable<Sugerencia> Sugerencias { get; set; }

    }
}
