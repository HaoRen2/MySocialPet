using MySocialPet.Models.Sugerencias;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Mascotas
{
    [Table("Raza")]
    public class Raza
    {
        public Raza()
        {
            Mascotas = new HashSet<Mascota>();
            RazasSugerencia = new HashSet<RazaSugerencia>();
        }

        [Key]
        public int IdRaza { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreRaza { get; set; }

        public string Informacion { get; set; }
        public string Tamanyo { get; set; }

        public string Foto { get; set; }
        public int? RatioIdeal { get; set; }

        public int IdEspecie { get; set; }
        public int IdCategoria { get; set; }

        [ForeignKey("IdEspecie")]
        public virtual Especie Especie { get; set; }

        [ForeignKey("IdCategoria")]
        public virtual Categoria Categoria { get; set; }

        public virtual ICollection<Mascota> Mascotas { get; set; }
        public virtual ICollection<RazaSugerencia> RazasSugerencia { get; set; }
    }

}
