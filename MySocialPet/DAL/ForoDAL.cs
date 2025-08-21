using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySocialPet.Models.Foros;

namespace MySocialPet.DAL
{
    public class ForoDAL
    {
        private readonly AppDbContexto _context;

        public ForoDAL(AppDbContexto context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        public async Task<List<Foro>> GetForosAsync()
        {
            return await _context.Foros.ToListAsync();
        }

        public async Task InsertForo(Foro foro)
        {
            _context.Foros.Add(foro);
            await _context.SaveChangesAsync();
        }

        public async Task<Foro?> GetForoByIdAsync(int id)
        {
            return await _context.Foros
                .FirstOrDefaultAsync(f => f.IdForo == id);
        }

        public async Task<List<Foro>> GetForosPorEspecieAsync(int idEspecie)
        {
            return await _context.Foros
                .Where(f => f.IdEspecie == idEspecie)
                .Include(f => f.Especie)
                .ToListAsync();
        }

        public async Task<List<DicusionForos>> GetDiscusionesPorForoAsync(int idForo)
        {
            return await _context.Discusiones
                .Where(d => d.IdForo == idForo)
                .Select(d => new DicusionForos
                {
                    Discusion = d,
                    CantidadMensajes = d.Mensajes.Count(),
                    FechaUltimoMensaje = d.Mensajes.Any()
                        ? d.Mensajes.Max(m => m.FechaEnvio)
                        : d.FechaCreacion
                }).ToListAsync();


        }

        public async Task<DiscusionMensajes?> GetDiscusionByIdAsync(int id)
        {
            return await _context.Discusiones
                .Where(x => x.IdDiscusion == id)
                .Select(d => new DiscusionMensajes
                {
                    IdForo = d.Foro.IdForo,
                    Titulo = d.Titulo,
                    Descripcion = d.Descripcion,
                    FechaCreacion = d.FechaCreacion,
                })
                .FirstOrDefaultAsync();
        }



        public async Task<List<SelectListItem>> GetForosPorEspecieSelectListAsync(int idEspecie)
        {
            return await _context.Foros
                .Where(f => f.IdEspecie == idEspecie)
                .Select(f => new SelectListItem
                {
                    Value = f.IdForo.ToString(),
                    Text = f.Nombre
                })
                .ToListAsync();
        }


        public async Task<Foro?> GetForoBySlugAsync(string slug)
        {
            return await _context.Foros
                .FirstOrDefaultAsync(f => f.Slug.ToLower() == slug.ToLower());
        }

        public async Task CrearHiloAsync(Discusion discusion)
        {
            _context.Discusiones.Add(discusion);
            await _context.SaveChangesAsync();
        }

        public async Task CrearMensaje(Mensaje mensaje)
        {
            _context.Mensajes.Add(mensaje);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Discusion>> GetTrendingDiscusionsAsync(int? foroId)
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
            if (foroId == null)
            {
                return await _context.Discusiones
                .Include(d => d.Mensajes)
                .Include(f => f.Foro)
                .Where(d => d.Mensajes.Any(m => m.FechaEnvio >= sevenDaysAgo))
                .OrderByDescending(d => d.Mensajes.Count(m => m.FechaEnvio >= sevenDaysAgo))
                .Take(5)
                .ToListAsync();
            }
            return await _context.Discusiones
                .Where(d => d.IdForo == foroId)
                .Include(d => d.Mensajes)
                .Include(f => f.Foro)
                .Where(d => d.Mensajes.Any(m => m.FechaEnvio >= sevenDaysAgo))
                .OrderByDescending(d => d.Mensajes.Count(m => m.FechaEnvio >= sevenDaysAgo))
                .Take(5)
                .ToListAsync();
        }

        public async Task<Mensaje?> GetMensajeById(int id)
        {
            return await _context.Mensajes
                .Include(m => m.Discusion)
                .ThenInclude(d => d.Foro)
                .FirstOrDefaultAsync(m => m.IdMensaje == id);
        }

        public async Task EliminarMensaje(int idMensaje)
        {
            var mensaje = await _context.Mensajes.FindAsync(idMensaje);
            if (mensaje != null)
            {
                _context.Mensajes.Remove(mensaje);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<(List<Mensaje> Mensajes, int Total)> GetMensajesByDiscusionAsync(int discId, string? ordenarPor, int page, int pageSize)
        {
            var query = _context.Mensajes
                .Where(m => m.IdDiscusion == discId)
                .Include(m => m.Usuario)
                .AsQueryable();

            // ordenar
            query = ordenarPor == "antiguos"
                ? query.OrderBy(m => m.FechaEnvio)
                : query.OrderByDescending(m => m.FechaEnvio);

            // total antes de aplicar paginación
            int total = await query.CountAsync();

            // aplicar paginación en SQL (Skip/Take)
            var mensajes = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (mensajes, total);
        }

    }
}