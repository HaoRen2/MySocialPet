using MySocialPet.Models.Mascotas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Salud
{
    [Table("PlanVacunacion")]
    public class PlanVacunacion
    {
        [Key]
        public int IdPlanVacunacion { get; set; }

        public int IdEspecie { get; set; }
        [ForeignKey("IdEspecie")]
        public virtual Especie Especie { get; set; }

        public int IdTipoVacuna { get; set; }
        [ForeignKey("IdTipoVacuna")]
        public virtual TipoVacuna TipoVacuna { get; set; }

        public int? EdadRecomendadaSemanas { get; set; }
        public bool EsRefuerzo { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }
    }
}
