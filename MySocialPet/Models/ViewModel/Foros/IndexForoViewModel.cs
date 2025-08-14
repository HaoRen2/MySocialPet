using MySocialPet.Models.Foros;

namespace MySocialPet.Models.ViewModel.Foros
{
    public class IndexForoViewModel
    {
        public List<Discusion>? Tendencias { get; set; }
        public List<Foro> Foros { get; set; } = new List<Foro>();
        public List<Discusion>? Discusiones { get; set; }
    }
}
