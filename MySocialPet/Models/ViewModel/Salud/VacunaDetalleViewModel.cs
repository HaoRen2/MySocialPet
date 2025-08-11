namespace MySocialPet.Models.ViewModel.Salud
{
  
        public class VacunaMascotaViewModel
        {
            public int IdMascota { get; set; }
            public string NombreMascota { get; set; }

            public string NombreEspecie { get; set; }

            public List<VacunaDetalleViewModel> Vacunas { get; set; }
        }

        public class VacunaDetalleViewModel
        {
            public string NombreVacuna { get; set; }
            public string? EdadRecomendada { get; set; }
            public bool EsRefuerzo { get; set; }
            public string Descripcion { get; set; }

            public bool Aplicada { get; set; }
            public bool Esencial { get; set; }
            public string Notas { get; set; }
            public DateTime? FechaAplicacion { get; set; }
        }
    
}
