using MySocialPet.Models.Foros;

namespace MySocialPet.Models.ViewModel.Foros
{
    public class ListaDiscusionesViewModel
    {
        public List<Discusion>? Tendencias { get; set; }
        public Foro Foro { get; set; } = new Foro();
        public List<Discusion>? Discusions { get; set; }
    }
}
