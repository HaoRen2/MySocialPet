using MySocialPet.Models.Autenticacion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Mascotas
{
    [Table("Protectora")]
    public class Protectora
    {
        [Key]
        public int IdProtectora { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Direccion { get; set; }  // opcional

        [Required, StringLength(100), EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        [Phone] // o usa Regex si quieres un formato concreto
        public string Telefono { get; set; }   // opcional

        [StringLength(100)]
        [Url]
        public string Web { get; set; }        // opcional

        // Relación con Usuario (requerida)
        [Required]
        public int IdUsuario { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public virtual Usuario Usuario { get; set; }

        // Auditoría / concurrencia
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [Timestamp] // Concurrency token (rowversion en SQL Server)
        public byte[] RowVersion { get; set; }
    }
}
