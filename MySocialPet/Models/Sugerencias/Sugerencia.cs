using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySocialPet.Models.Sugerencias
{
    [Table("Sugerencia")]
    public class Sugerencia
    {
        public Sugerencia()
        {
            EspeciesSugerencia = new HashSet<EspecieSugerencia>();
            RazasSugerencia = new HashSet<RazaSugerencia>();
            CategoriaSugerencias = new HashSet<CategoriaSugerencia>();
        }

        [Key]
        public int IdSugerencia { get; set; }

        [Required]
        [StringLength(255)]
        public string Titulo { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [StringLength(100)]
        public string Tema { get; set; }

        public virtual ICollection<EspecieSugerencia> EspeciesSugerencia { get; set; }
        public virtual ICollection<RazaSugerencia> RazasSugerencia { get; set; }
        public virtual ICollection<CategoriaSugerencia> CategoriaSugerencias { get; set; }

    }

}
