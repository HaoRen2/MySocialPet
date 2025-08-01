using MySocialPet.Models.Mascotas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Salud
{
    [Table("Evento")]
    public class Evento
    {
        [Key]
        public int IdEvento { get; set; }

        [Required]
        [StringLength(255)]
        public string Titulo { get; set; }

        public DateTime FechaHora { get; set; }

        [Required]
        [StringLength(100)]
        public string TipoEvento { get; set; }

        public string Notas { get; set; }
        public bool NotificacionEnviada { get; set; }

        public int IdMascota { get; set; }
        [ForeignKey("IdMascota")]
        public virtual Mascota Mascota { get; set; }
    }
}
