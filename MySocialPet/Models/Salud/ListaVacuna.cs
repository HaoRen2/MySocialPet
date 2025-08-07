using MySocialPet.Models.Mascotas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Salud
{
    [Table("ListaVacuna")]
    public class ListaVacuna
    {
        [Key]
        public int IdListaVacuna { get; set; }

        public int IdEspecie { get; set; }
        [ForeignKey("IdEspecie")]
        public virtual Especie Especie { get; set; }

        public int IdTipoVacuna { get; set; }
        [ForeignKey("IdTipoVacuna")]
        public virtual TipoVacuna TipoVacuna { get; set; }

        public string EdadRecomendada { get; set; }
        public bool EsRefuerzo { get; set; }

        public string Descripcion { get; set; }
        public string Notas { get; set; }
        public bool Esencial { get; set; }

    }
}
