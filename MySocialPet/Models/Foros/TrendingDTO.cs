namespace MySocialPet.Models.Foros
{
    public class TrendingDTO
    {
        public int IdDiscusion { get; set; }
        public int IdForo { get; set; }
        public string Titulo { get; set; }
        public string SlugForo { get; set; }
        public int CantidadMensajes { get; set; }
    }
}
