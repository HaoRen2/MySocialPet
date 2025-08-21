

using MySocialPet.Models.Albums;
using MySocialPet.Models.Foros;

namespace MySocialPet.Models.ViewModel.Albums
{
    public class DetailAlbumViewModel
    {
        public int IdAlbum { get; set; }
        public string NombreAlbum { get; set; }
        public int TotalFotos { get; set; } 

        public List<FotoAlbum> FotosDeLaPagina { get; set; }

        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }

        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;
    }
}
