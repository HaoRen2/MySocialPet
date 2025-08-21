using MySocialPet.Models.Foros;

namespace MySocialPet.Models.ViewModel.Foros
{
    public class ListaDiscusionesViewModel
    {
        public string Slug { get; set; }
        public int IdForo { get; set; }
        public int IdDiscusion { get; set; }
        public List<Discusion>? Tendencias { get; set; }
        public Foro Foro { get; set; } = new Foro();
        public List<DicusionForos>? Discusions { get; set; }
    }
}
