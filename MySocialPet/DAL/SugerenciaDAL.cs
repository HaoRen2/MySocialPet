using MySocialPet.Models.Sugerencias;

namespace MySocialPet.DAL
{
    public class SugerenciaDAL
    {
        private readonly AppDbContexto _context;

        public SugerenciaDAL(AppDbContexto context)
        {
            _context = context;
        }

        public List<Sugerencia> getAllSugerencia() 
        {
            return _context.Sugerencias.ToList();
        }
    }
}
