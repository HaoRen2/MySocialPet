namespace MySocialPet.DAL
{
    public class SaludDAL
    {
        private readonly AppDbContexto _context;

        public SaludDAL(AppDbContexto context)
        {
            _context = context;
        }
    }
}
