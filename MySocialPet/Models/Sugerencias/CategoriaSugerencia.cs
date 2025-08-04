using MySocialPet.Models.Mascotas;

namespace MySocialPet.Models.Sugerencias
{
    public class CategoriaSugerencia
    {
        public int IdCategoria { get; set; }
        public int IdSugerencia { get; set; }

        public virtual Categoria Categoria { get; set; }
        public virtual Sugerencia Sugerencia { get; set; }
    }
}
