using MySocialPet.Models.Albums;
using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Albums
{
    public class ListaAlbumViewModel
    {
        public List<Album> ListAlbums { get; set; } = new List<Album>();

    }
}
