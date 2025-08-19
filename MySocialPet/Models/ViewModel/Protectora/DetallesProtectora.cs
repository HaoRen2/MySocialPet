namespace MySocialPet.Models.ViewModel.Protectora
{
    public class DetallesProtectora
    {
        // === Campos BD ===
        public int IdProtectora { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Web { get; set; }
        public int IdUsuario { get; set; }

        // === Presentación / opcionales ===
        public string LogoUrl { get; set; }             // opcional
        public string DescripcionCorta { get; set; }    // opcional
        public string HorarioHumanizado { get; set; }   // opcional

        // === Ubicación (opcionales) ===
        public decimal? Latitud { get; set; }           // opcional en BD
        public decimal? Longitud { get; set; }          // opcional en BD

        // === KPIs (derivados) ===
        public int NumGatos { get; set; }
        public int NumPerros { get; set; }
        public int NumMascotasTotales { get; set; }

        // === Pestaña Mascotas + filtros/paginación ===
        public IEnumerable<MascotaAdopcionViewModel> Mascotas { get; set; }
        public string Busqueda { get; set; }            // filtro “Buscar mascota…”
        public string FiltroTipo { get; set; }          // "Todos" | "Perro" | "Gato"
        public string Orden { get; set; }               // "recientes" | "A-Z" | "Edad"
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        // Enlaces
        public string UrlWebExterna { get; set; }
        public string UrlEmailMailto { get; set; }
    }
}
