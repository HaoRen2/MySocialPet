namespace MySocialPet.Models.ViewModel.Albums
{
    public class CrearAlbumViewModel
    {
        public string NombreAlbum { get; set; } = string.Empty;
        public int IdUsuario { get; set; }
        public byte[]? FotoReciente { get; set; } // Optional, can be null if no photo is uploaded
        public List<IFormFile>? Fotos { get; set; } = new List<IFormFile>(); // List of photos to upload


        public int? IdMascota { get; set; } // Optional, can be used to associate the album with a pet
        public string? NombreMascota { get; set; } = string.Empty; // Optional, can be used to display the pet's name in the album
        /*public class EtiquetaMascota
        {
            public int IdMascota { get; set; } // Optional, can be used to associate the album with a pet
            public string NombreMascota { get; set; } = string.Empty; // Optional, can be used to display the pet's name
        }*/
    }
}
