using MySocialPet.Models.Autenticacion;
using MySocialPet.Models.Mascotas;

namespace MySocialPet.Models.ViewModel
{
    public class DetailMascotaViewModel
    {
        public Mascota? DetailMascota { get; set; } = new Mascota();

        public string Descripcion { get; set; }
        public List<Nota>? Notas { get; set; }
    }
}
