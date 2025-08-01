using MySocialPet.Models.Autenticacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Foros
{
    [Table("Discusion")]
    public class Discusion
    {
        public Discusion()
        {
            Mensajes = new HashSet<Mensaje>();
        }

        [Key]
        public int IdDiscusion { get; set; }

        [Required]
        [StringLength(255)]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool EstaFinalizado { get; set; }

        public int IdForo { get; set; }
        [ForeignKey("IdForo")]
        public virtual Foro Foro { get; set; }

        public int IdUsuarioCreador { get; set; }
        [ForeignKey("IdUsuarioCreador")]
        public virtual Usuario UsuarioCreador { get; set; }

        public virtual ICollection<Mensaje> Mensajes { get; set; }
    }
}
