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

        // =======================
        // Identidad y autenticación
        // =======================
        [Key]
        public int IdUsuario { get; set; }

        [Required, StringLength(100), EmailAddress]
        public string Email { get; set; }

        public byte[]? AvatarFoto { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaRegistro { get; set; }

        // Rol / tipo de usuario
        public int IdTipoUsuario { get; set; }

        [ForeignKey(nameof(IdTipoUsuario))]
        public virtual TipoUsuario TipoUsuario { get; set; }

        // =======================
        // Relaciones de navegación
        // =======================
        public virtual ICollection<Mascota> Mascotas { get; set; }
        public virtual ICollection<Album> Albumes { get; set; }
        public virtual ICollection<Discusion> DiscusionesCreadas { get; set; }
        public virtual ICollection<Mensaje> Mensajes { get; set; }

        // =======================
        // Recuperación de contraseña
        // (Se usan para generar y validar el enlace de "Olvidé mi contraseña".
        //  ResetToken: token único enviado por email.
        //  TokenExpiration: momento límite (UTC) hasta el que el token es válido.)
        // =======================
        [StringLength(100)]
        public string? ResetToken { get; set; }

        public DateTime? TokenExpiration { get; set; }

        // =======================
        // Información adicional del usuario (opcional / editable en Perfil)
        // =======================
        [StringLength(100)]
        public string? Nombre { get; set; }

        [StringLength(100)]
        public string? Apellido { get; set; }

        [StringLength(30)]
        [DataType(DataType.PhoneNumber)]
        public string? Telefono { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }

        [StringLength(100)]
        public string? Ciudad { get; set; }

        [StringLength(100)]
        public string? Provincia { get; set; }

        [StringLength(20)]
        public string? CodigoPostal { get; set; }
    }
}
