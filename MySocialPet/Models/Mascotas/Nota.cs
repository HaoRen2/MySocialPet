using MySocialPet.Models.Sugerencias;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Mascotas
{
    [Table("Nota")]
    public class Nota
    {
        [Key]
        public int IdNota { get; set; }

        [Required]
        [StringLength(150)]
        public string Descripcion { get; set; }

        public int IdMascota { get; set; }
        [ForeignKey("IdMascota")]
        public virtual Mascota Mascota { get; set; }

    }
}
