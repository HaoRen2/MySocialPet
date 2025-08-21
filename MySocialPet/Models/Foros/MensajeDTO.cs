namespace MySocialPet.Models.Foros
{
    public class MensajeDTO
    {
        public int IdMensaje { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime FechaEnvio { get; set; }
        public string ContenidoMensaje { get; set; } = string.Empty;
        public byte[]? Imagen { get; set; }
        public byte[]? AvatarFoto { get; set; }
    }
}
