using MySocialPet.Models.Mascotas;

namespace MySocialPet.Models.ViewModel.Mascotas
{
    public class MascotaSelectorViewModel
    {
        public List<Mascota> Mascotas { get; set; }
        public int MascotaIdActual { get; set; }
        public string Controlador { get; set; }
        public string Accion { get; set; }
    }
}
