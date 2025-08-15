using Microsoft.EntityFrameworkCore;
using MySocialPet.Models.Albums;
using MySocialPet.Models.Mascotas;
using MySocialPet.Models.Salud;

namespace MySocialPet.DAL
{
    public class SaludDAL
    {
        private readonly AppDbContexto _context;

        public SaludDAL(AppDbContexto context)
        {
            _context = context;
        }

        public Mascota GetEventosMascota(int id)
        {
            var mascota = _context.Mascotas
            .Include(m => m.Eventos)
            .Include(m => m.Raza)
                .ThenInclude(r => r.Especie)
            .FirstOrDefault(m => m.IdMascota == id);

            return mascota;
        }


        public async Task InsertMascota(Evento evento)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
        }


    }
}
