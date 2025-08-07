using MySocialPet.Models.Foros;

namespace MySocialPet.Models.ViewModel.Foros
{
    public class DetailDiscusionViewModel
    {
        public int IdForo { get; set; } 
        public Discusion? Discusion { get; set; } = new Discusion();
    }
}
