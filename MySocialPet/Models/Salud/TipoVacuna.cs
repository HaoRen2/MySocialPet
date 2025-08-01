using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Salud
{
    [Table("TipoVacuna")]
    public class TipoVacuna
    {
        [Key]
        public int IdTipoVacuna { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
    }
}
