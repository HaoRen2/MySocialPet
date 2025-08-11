using MySocialPet.Models.Mascotas;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Sugerencias
{
    [Table("CategoriaSugerencia")]

    public class CategoriaSugerencia
    {
        public int IdCategoria { get; set; }
        public int IdSugerencia { get; set; }

        public virtual Categoria Categoria { get; set; }
        public virtual Sugerencia Sugerencia { get; set; }
    }
}
