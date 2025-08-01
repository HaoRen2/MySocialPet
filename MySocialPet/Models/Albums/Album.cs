using MySocialPet.Models.Autenticacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Albums
{
    [Table("Album")]
    public class Album
    {
        public Album()
        {
            Fotos = new HashSet<FotoAlbum>();
        }

        [Key]
        public int IdAlbum { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreAlbum { get; set; }

        public int IdUsuario { get; set; }
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<FotoAlbum> Fotos { get; set; }
    }
}
