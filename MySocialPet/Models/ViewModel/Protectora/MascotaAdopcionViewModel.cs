namespace MySocialPet.Models.ViewModel.Protectora
{
    public class MascotaAdopcionViewModel
    {
        // --- Campos 1:1 con BD (o navegación directa) ---
        public int IdMascota { get; set; }
        public string Nombre { get; set; }
        public DateTime? Nacimiento { get; set; }
        public decimal? PesoKg { get; set; }
        public decimal? LongitudCm { get; set; }
        public string Genero { get; set; }                 // "Macho"/"Hembra" o como lo guardes
        public int? BCS { get; set; }
        public bool Esterilizada { get; set; }
        public string EstadoAdopcion { get; set; }         // tu string actual ("Disponible","Reservada", etc.)
        public int IdUsuario { get; set; }                 // para saber a qué protectora (por IdUsuario) pertenece
        public int? IdRaza { get; set; }                   // puede venir null si lo permites

        // --- Derivados para UI (no requieren cambios en BD) ---
        public int? EdadAnios { get; set; }
        public int? EdadMesesResto { get; set; }
        public string RazaNombre { get; set; }             // from m.Raza?.Nombre
        public string EspecieNombre { get; set; }          // from m.Raza?.Especie?.Nombre (si existe)
        public bool VacunasAlDia { get; set; }             // cálculo simple (ver mapper)

        // --- Presentación ---
        public string DescripcionCorta { get; set; }       // p.ej. 1ª nota o texto fijo si no hay
        public string FotoDataUrl { get; set; }            // "data:image/png;base64,..." o null para usar avatar por defecto

        // --- Conveniencia enlaces ---
        public string UrlFicha { get; set; }
        public string UrlAdoptar { get; set; }
    }
}
