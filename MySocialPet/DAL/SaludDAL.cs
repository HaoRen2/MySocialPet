using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<Mascota?> GetMascotaAsync(int id)
        {
            return await _context.Mascotas
                .Include(m => m.Raza)
                .ThenInclude(r => r.Especie)
                .FirstOrDefaultAsync(m => m.IdMascota == id);
        }

        public async Task<List<ListaVacuna>> GetListaVacunasAsync(int idEspecie)
        {
            return await _context.ListaVacunas
                .Include(lv => lv.TipoVacuna)
                .Where(lv => lv.IdEspecie == idEspecie)
                .ToListAsync();
        }

        public async Task<List<VacunaRegistro>> GetVacunasRegistradasAsync(int idMascota)
        {
            return await _context.VacunaRegistros
                .Include(v => v.TipoVacuna)
                .Where(v => v.IdMascota == idMascota)
                .ToListAsync();
        }

        public async Task<List<SelectListItem>> GetVacunasDisponiblesAsync(List<int> vacunasRegistradasIds, int idEspecie)
        {
            return await _context.ListaVacunas.Where(lv => lv.IdEspecie == idEspecie && !vacunasRegistradasIds.Contains(lv.IdTipoVacuna))
            .Select(lv => lv.TipoVacuna) 
            .Distinct()
            .OrderBy(v => v.Nombre)
            .Select(v => new SelectListItem
            {
               Value = v.IdTipoVacuna.ToString(),
                Text = v.Nombre
            }).ToListAsync();
        }

    }
}
