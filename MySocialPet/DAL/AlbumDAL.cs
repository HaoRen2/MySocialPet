using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySocialPet.Models.Albums;
using MySocialPet.Models.ViewModel.Albums;

namespace MySocialPet.DAL
{
    public class AlbumDAL
    {
        private readonly AppDbContexto _context;

        public AlbumDAL(AppDbContexto context)
        {
            _context = context;
        }
        public List<Album> GetAlbumesPorUsuario(string userId)
        {
            int id = int.Parse(userId);

            return _context.Albumes
                .Where(a => a.IdUsuario.ToString() == userId)
                .Include(a => a.Fotos)
                .ToList();
        }
        public Album GetAlbumPorId(int idAlbum)
        {
            return _context.Albumes
                .Include(a => a.Fotos)
                .FirstOrDefault(a => a.IdAlbum == idAlbum);
        }
        public byte[] GetFotoRecinete(string IdAlbum)
        {
            int id = int.Parse(IdAlbum);
            var fotoReciente = _context.FotoAlbumes
                .Where(f => f.IdAlbum == id)
                .OrderByDescending(f => f.Fecha)
                .FirstOrDefault();
            if (fotoReciente != null)
            {
                // Retorna la foto más reciente del álbum
                return fotoReciente.Foto;
            }
            // Retorna un arreglo vacío si no hay fotos recientes
            return Array.Empty<byte>(); 

        }

        public List<SelectListItem> GetListaNombreMascotasPorUsuario(int userId)
        {
            return _context.Mascotas
                .Where(m => m.IdUsuario == userId)
                .Select(m => new SelectListItem
                {
                    Value = m.IdMascota.ToString(),
                    Text = m.Nombre.ToString()
                }).ToList();
        }

        internal async Task InsertAlbum(CrearAlbumViewModel model)
        {
            var album = new Album
            {
                NombreAlbum = model.NombreAlbum,
                IdUsuario = model.IdUsuario,
            };
            _context.Albumes.Add(album);
            await _context.SaveChangesAsync();
        }
    }
}

