namespace MySocialPet.Models.ViewModel.Salud
{

    public class CalendarioEventosViewModel
    {
        public int IdMascota { get; set; }
        public string NombreMascota { get; set; }
        public string Especie { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public List<EventoViewModel>? Eventos { get; set; }
    }

    public class EventoViewModel
    {
        public int? IdEvento { get; set; }
        public int IdMascota { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaHora { get; set; }
        public string Color { get; set; }
        public string Notas { get; set; }
    }

}
