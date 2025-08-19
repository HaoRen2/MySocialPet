using MySocialPet.Models.Mascotas;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Albums
{
    [Table("FotoEtiquetaMascota")]
    public class FotoEtiquetaMascota
    {
        // Clave primaria compuesta configurada en ApplicationDbContext
        public int? IdFoto { get; set; }
        public int? IdMascota { get; set; }

        public virtual FotoAlbum? FotoAlbum { get; set; }
        public virtual Mascota? Mascota { get; set; }
    }

}
