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
            _context = context;
        }

        public List<Foro> GetForos()
        {
            return _context.Foros.ToList();
        }

        public async Task InsertForo(Foro foro)
        {
            _context.Foros.Add(foro);
            await _context.SaveChangesAsync();
        }
        public Foro? GetForoById(int id)
        {
            return _context.Foros
                .FirstOrDefault(f => f.IdForo == id);
        }
        public List<Foro> GetForosPorEspecie(int idEspecie)
        {
            return _context.Foros
                .Where(f => f.IdEspecie == idEspecie)
                .Include(f => f.Especie)
                .ToList();
        }

        public List<Discusion> GetDiscusionesPorForo(int idForo)
        {
            return _context.Discusiones
                .Where(d => d.IdForo == idForo)
                .Include(d => d.UsuarioCreador)
                .ToList();
        }

        public Discusion? GetDiscusionById(int id)
        {
            return _context.Discusiones
                .Include(d => d.Mensajes)
                .Include(d => d.UsuarioCreador)
                .FirstOrDefault(x => x.IdDiscusion == id);
        }

        public List<SelectListItem> GetForosPorEspecieSelectList(int idEspecie)
        {
            return _context.Foros
                .Where(f => f.IdEspecie == idEspecie)
                .Select(f => new SelectListItem
                {
                    Value = f.IdForo.ToString(),
                    Text = f.Nombre
                })
                .ToList();
        }

        public Foro? GetForoBySlug(string slug)
        {
            return _context.Foros
                .Include(f => f.Discusiones)
                    .ThenInclude(d => d.Mensajes)
                .FirstOrDefault(f => f.Slug.ToLower() == slug.ToLower());
        }

        public void CrearHilo(Discusion discusion)
        {
            _context.Discusiones.Add(discusion);
            _context.SaveChanges();
        }
        public async Task CrearMensaje(Mensaje mensaje)
        {
            _context.Mensajes.Add(mensaje);
            await _context.SaveChangesAsync();
        }

    }
}
