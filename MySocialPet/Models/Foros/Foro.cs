using MySocialPet.Models.Mascotas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Foros
{
    [Table("Foro")]
    public class Foro
    {
        public Foro()
        {
            Discusiones = new HashSet<Discusion>();
        }

        [Key]
        public int IdForo { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string? Icono { get; set; }
        public string Slug { get; set; }
        public int? IdEspecie { get; set; }

        [ForeignKey("IdEspecie")]
        public virtual Especie Especie { get; set; }
        public virtual ICollection<Discusion> Discusiones { get; set; }
    }
}
