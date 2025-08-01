using MySocialPet.Models.Mascotas;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Sugerencias
{
    [Table("EspecieSugerencia")]
    public class EspecieSugerencia
    {
        // Clave primaria compuesta configurada en ApplicationDbContext
        public int IdEspecie { get; set; }
        public int IdSugerencia { get; set; }

        public virtual Especie Especie { get; set; }
        public virtual Sugerencia Sugerencia { get; set; }
    }
}
