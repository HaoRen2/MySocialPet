using MySocialPet.Models.Mascotas;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Sugerencias
{
    [Table("RazaSugerencia")]
    public class RazaSugerencia
    {
        // Clave primaria compuesta configurada en ApplicationDbContext
        public int IdRaza { get; set; }
        public int IdSugerencia { get; set; }

        public virtual Raza Raza { get; set; }
        public virtual Sugerencia Sugerencia { get; set; }
    }
}
