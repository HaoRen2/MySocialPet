using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Mascotas
{
    [Table("Especie")]
    public class Especie
    {
        public Especie()
        {
            Razas = new HashSet<Raza>();
        }

        [Key]
        public int IdEspecie { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public virtual ICollection<Raza> Razas { get; set; }
    }
}
