using Microsoft.AspNetCore.Mvc.Rendering;

namespace MySocialPet.Models.ViewModel.Albums
{
    public class EditarFotoViewModel
    {
        public int IdFoto { get; set; }
        public int IdAlbum { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public byte[]? NuevaFoto { get; set; } 
        public List<int?>? MascotasEtiquetadasIds { get; set; }
        public List<SelectListItem>? MascotasDisponibles { get; set; }
    }
}
