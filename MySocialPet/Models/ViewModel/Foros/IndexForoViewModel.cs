using Microsoft.AspNetCore.Mvc.ModelBinding;
using MySocialPet.Models.Foros;

namespace MySocialPet.Models.ViewModel.Foros
{
    public class IndexForoViewModel
    {
        public List<Foro> Foros { get; set; } = new List<Foro>();

        public List<Discusion>? Discusiones { get; set; }
    }
}
