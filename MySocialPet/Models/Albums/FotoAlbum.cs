using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Albums
{
    [Table("FotoAlbum")]
    public class FotoAlbum
    {
        public FotoAlbum()
        {
            MascotasEtiquetadas = new HashSet<FotoEtiquetaMascota>();
        }

        [Key]
        public int IdFoto { get; set; }

        [StringLength(100)]
        public string Titulo { get; set; }

        [Required]
        public byte[] Foto { get; set; }

        public string Descripcion { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public int IdAlbum { get; set; }
        [ForeignKey("IdAlbum")]
        public virtual Album Album { get; set; }

        public virtual ICollection<FotoEtiquetaMascota> MascotasEtiquetadas { get; set; } //(puede ser NULL) hacer un desplegable en insertar foto con las mascotas del user
    }
}
