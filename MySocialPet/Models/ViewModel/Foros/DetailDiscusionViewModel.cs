using MySocialPet.Models.Foros;

namespace MySocialPet.Models.ViewModel.Foros
{
    public class DetailDiscusionViewModel
    {
        public string Slug { get; set; }
        public int IdForo { get; set; }
        public int IdDiscusion { get; set; }
        public Discusion? Discusion { get; set; } = new Discusion();
        public List<Discusion>? Tendencias { get; set; }

    }
}
