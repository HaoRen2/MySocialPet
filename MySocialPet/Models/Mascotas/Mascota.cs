using MySocialPet.Models.Albums;
using MySocialPet.Models.Autenticacion;
using MySocialPet.Models.Salud;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Mascotas
{
    [Table("Mascota")]
    public class Mascota
    {
        public Mascota()
        {
            SaludRegistros = new HashSet<SaludRegistro>();
            VacunaRegistros = new HashSet<VacunaRegistro>();
            Eventos = new HashSet<Evento>();
            FotosEtiquetadas = new HashSet<FotoEtiquetaMascota>();
        }

        [Key]
        public int IdMascota { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Nacimiento { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? PesoKg { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal? LongitudCm { get; set; }

        [StringLength(10)]
        public string Genero { get; set; }

        public byte[] Foto { get; set; }

        [Column(TypeName = "decimal(3, 1)")]
        public decimal? BCS { get; set; }

        public bool Esterilizada { get; set; }

        [Required]
        [StringLength(50)]
        public string EstadoAdopcion { get; set; }

        public int? IdUsuario { get; set; }
        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

        public int IdRaza { get; set; }
        [ForeignKey("IdRaza")]
        public virtual Raza Raza { get; set; }

        public int? IdProtectora { get; set; }
        [ForeignKey("IdProtectora")]
        public virtual Protectora Protectora { get; set; }

        public virtual ICollection<SaludRegistro> SaludRegistros { get; set; }
        public virtual ICollection<VacunaRegistro> VacunaRegistros { get; set; }
        public virtual ICollection<Evento> Eventos { get; set; }
        public virtual ICollection<FotoEtiquetaMascota> FotosEtiquetadas { get; set; }
  
    }
}
