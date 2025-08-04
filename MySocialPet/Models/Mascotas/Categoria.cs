using MySocialPet.Models.Sugerencias;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Mascotas
{
    [Table("Categoria")]
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required]
        [StringLength(150)]
        public string NombreCategoria { get; set; }

        public int IdEspecie { get; set; }
        [ForeignKey("IdEspecie")]
        public virtual Especie Especie { get; set; }

        public virtual ICollection<Raza> Razas { get; set; }
        public virtual ICollection<CategoriaSugerencia> CategoriaSugerencias { get; set; }
    }

}
