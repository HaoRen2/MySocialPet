using MySocialPet.Models.Mascotas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Salud
{
    [Table("VacunaRegistro")]
    public class VacunaRegistro
    {
        [Key]
        public int IdVacunaRegistro { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public int IdMascota { get; set; }
        [ForeignKey("IdMascota")]
        public virtual Mascota Mascota { get; set; }

        public int IdTipoVacuna { get; set; }
        [ForeignKey("IdTipoVacuna")]
        public virtual TipoVacuna TipoVacuna { get; set; }
    }
}
