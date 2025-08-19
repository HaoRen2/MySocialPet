using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MySocialPet.Models.Autenticacion;

namespace MySocialPet.Models.Protectora
{
    [Table("Protectora")]
    public class Protectora
    {
        [Key]
        public int IdProtectora { get; set; }              // PK

        [Required, StringLength(100)]
        public string Nombre { get; set; } = string.Empty; // NOT NULL

       
        [StringLength(100)]
        [Url]
        public string? Web { get; set; }                   // NULL

        [Required]
        public int IdUsuario { get; set; }                 // NOT NULL (FK)

        [ForeignKey(nameof(IdUsuario))]
        public virtual Usuario? Usuario { get; set; }      // navegación (puede no cargarse)

        [Required]
        public bool Activo { get; set; } = true;           // NOT NULL

        public string Descripcion { get; set; } = string.Empty; // NOT NULL
        public string Introduccion { get; set; } = string.Empty; // NOT NULL

        // Opcional: valor por defecto para Activo
        public Protectora()
        {
            Activo = true;
        }
    }
}
