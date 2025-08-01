using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Mascotas
{
    [Table("Especie")]
    public class Especie
    {
        public Especie()
        {
            Razas = new HashSet<Raza>();
            PlanesVacunacion = new HashSet<PlanVacunacion>();
            EspeciesSugerencia = new HashSet<EspecieSugerencia>();
        }

        [Key]
        public int IdEspecie { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public virtual ICollection<Raza> Razas { get; set; }
        public virtual ICollection<PlanVacunacion> PlanesVacunacion { get; set; }
        public virtual ICollection<EspecieSugerencia> EspeciesSugerencia { get; set; }
    }
}
