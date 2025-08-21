using MySocialPet.Models.Autenticacion;

namespace MySocialPet.Models.Foros
{
    public class DiscusionMensajes
    {
        public int IdForo { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public List<MensajeDTO>? Mensajes { get; set; }
    }
}
