using MySocialPet.Models.Mascotas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Salud
{
    [Table("SaludRegistro")]
    public class SaludRegistro
    {
        [Key]
        public int IdSalud { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? PesoKg { get; set; }

        public int? BCS { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal LongitudCm { get; set; }

        public int IdMascota { get; set; }
        [ForeignKey("IdMascota")]
        public virtual Mascota Mascota { get; set; }
    }
}
