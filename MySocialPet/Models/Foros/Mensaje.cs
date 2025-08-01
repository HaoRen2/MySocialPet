using MySocialPet.Models.Autenticacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Foros
{
    [Table("Mensaje")]
    public class Mensaje
    {
        [Key]
        public int IdMensaje { get; set; }

        [Required]
        public string ContenidoMensaje { get; set; }
        public DateTime FechaEnvio { get; set; }

        public int IdUsuario { get; set; }
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

        public int IdDiscusion { get; set; }
        [ForeignKey("IdDiscusion")]
        public virtual Discusion Discusion { get; set; }
    }

}
