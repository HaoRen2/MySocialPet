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
            var discusionData = await _context.Discusiones
                .Where(d => d.IdForo == idForo)
                .Select(d => new
                {
                    Discusion = d,
                    CantidadMensajes = d.Mensajes.Count(),
                    FechaUltimoMensaje = d.Mensajes.Any() ? d.Mensajes.Max(m => m.FechaEnvio) : d.FechaCreacion
                }).ToListAsync();

            return discusionData.Select(d => new DicusionForos
            {
                Discusion = d.Discusion,
                CantidadMensajes = d.CantidadMensajes,
                FechaUltimoMensaje = d.FechaUltimoMensaje
            }).ToList();
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

        public async Task<List<TrendingDTO>> GetTrendingDiscusionsAsync(int? foroId)
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

            var query = _context.Discusiones
                .Where(d => d.Mensajes.Any(m => m.FechaEnvio >= sevenDaysAgo))
                .AsQueryable();

            if (foroId.HasValue)
            {
                query = query.Where(d => d.IdForo == foroId.Value);
            }

            // Proyecta la consulta para traer solo los campos que necesitas
            return await query
                .OrderByDescending(d => d.Mensajes.Count(m => m.FechaEnvio >= sevenDaysAgo))
                .Take(5)
                .Select(d => new TrendingDTO
                {
                    IdDiscusion = d.IdDiscusion,
                    IdForo = d.IdForo,
                    Titulo = d.Titulo,
                    SlugForo = d.Foro.Slug, // Accede al slug a través de la relación, EF lo traducirá a un JOIN
                    CantidadMensajes = d.Mensajes.Count() // Esto sigue siendo lento si no hay índice
                })
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

        public async Task<(List<MensajeDTO> Mensajes, int Total)> GetMensajesByDiscusionAsync(int discId, string? ordenarPor, int page, int pageSize)
        {
            var query = _context.Mensajes
                .Where(m => m.IdDiscusion == discId);

            var orderedQuery = ordenarPor == "antiguos"
                ? query.OrderBy(m => m.FechaEnvio)
                : query.OrderByDescending(m => m.FechaEnvio);

            var pagedAndProjectedQuery = orderedQuery
                .Select(m => new MensajeDTO
                {
                    IdMensaje = m.IdMensaje,
                    Username = m.Usuario.Username,
                    FechaEnvio = m.FechaEnvio,
                    ContenidoMensaje = m.ContenidoMensaje,
                    Imagen = m.Imagen != null && m.Imagen.Length > 0,
                    AvatarFoto = m.Usuario.AvatarFoto
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            var mensajes = await pagedAndProjectedQuery.ToListAsync();
            int total = await query.CountAsync();

            return (mensajes, total);
        }
        public async Task<Mensaje?> GetMensajeByIdAsync(int id)
        {
            return await _context.Mensajes
                .Where(m => m.IdMensaje == id)
                .Select(m => new Mensaje
                {
                    IdMensaje = m.IdMensaje,
                    Imagen = m.Imagen
                })
                .FirstOrDefaultAsync();
        }
    }
}