using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Albums
{
    public class EditarAlbumViewModel
    {
        public int IdAlbum { get; set; }

        [Required(ErrorMessage = "El nombre del álbum es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        [Display(Name = "Nombre del Álbum")]
        public string NombreAlbum { get; set; }
    }
}
