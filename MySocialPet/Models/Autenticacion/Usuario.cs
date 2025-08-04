using MySocialPet.Models.Albums;
using MySocialPet.Models.Foros;
using MySocialPet.Models.Mascotas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Autenticacion
{
    [Table("Usuario")]
    public class Usuario
    {
        public Usuario()
        {
            Mascotas = new HashSet<Mascota>();
            Albumes = new HashSet<Album>();
            DiscusionesCreadas = new HashSet<Discusion>();
            Mensajes = new HashSet<Mensaje>();
        }

        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        public byte[]? AvatarFoto { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaRegistro { get; set; }

        public int IdTipoUsuario { get; set; }
        [ForeignKey("IdTipoUsuario")]
        public virtual TipoUsuario TipoUsuario { get; set; }

        public virtual ICollection<Mascota> Mascotas { get; set; }
        public virtual ICollection<Album> Albumes { get; set; }
        public virtual ICollection<Discusion> DiscusionesCreadas { get; set; }
        public virtual ICollection<Mensaje> Mensajes { get; set; }
        public virtual Protectora Protectora { get; set; }

        public string? ResetToken { get; set; }
        public DateTime? TokenExpiration { get; set; }
    }
}
