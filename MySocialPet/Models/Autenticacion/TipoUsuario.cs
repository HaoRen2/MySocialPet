using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Autenticacion
{
    [Table("TipoUsuario")]
    public class TipoUsuario
    {
        public TipoUsuario()
        {
            Usuarios = new HashSet<Usuario>();
        }

        [Key]
        public int IdTipoUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreTipo { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
