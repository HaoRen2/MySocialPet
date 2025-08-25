using MySocialPet.Models.Autenticacion;
using MySocialPet.Models.Mascotas;
using MySocialPet.Models.Salud;

namespace MySocialPet.Models.ViewModel
{
    public class DetailMascotaViewModel
    {
        public Mascota? DetailMascota { get; set; } = new Mascota();
        public string Descripcion { get; set; }

        public string? RazaFoto { get; set; }


        public List<Nota>? Notas { get; set; }
        public Evento Evento { get; set; }
    }
}
