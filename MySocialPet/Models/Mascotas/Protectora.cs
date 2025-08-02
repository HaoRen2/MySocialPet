using MySocialPet.Models.Autenticacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Mascotas
{
    [Table("Protectora")]
    public class Protectora
    {
        public Protectora()
        {
        }

        [Key]
        public int IdProtectora { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Direccion { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(100)]
        [Url]
        public string Web { get; set; }

        public int IdUsuario { get; set; }
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

    }
}
