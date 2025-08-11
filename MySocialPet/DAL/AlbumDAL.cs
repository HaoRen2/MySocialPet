using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySocialPet.Models.Albums;
using MySocialPet.Models.ViewModel.Albums;
using MySocialPet.Models.ViewModel.Foros;

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

        public byte[] GetFotoReciente(string IdAlbum)
        {
            int id = int.Parse(IdAlbum);
            var fotoReciente = _context.FotoAlbumes
                .Where(f => f.IdAlbum == id)
                .OrderByDescending(f => f.Fecha)
                .FirstOrDefault();

            return fotoReciente?.Foto ?? Array.Empty<byte>();
        }

        public List<SelectListItem> GetListaNombreMascotasPorUsuario(int userId)
        {
            return _context.Mascotas
                .Where(m => m.IdUsuario == userId)
                .Select(m => new SelectListItem
                {
                    Value = m.IdMascota.ToString(),
                    Text = m.Nombre
                }).ToList();
        }

        public async Task<int> InsertAlbum(CrearAlbumViewModel model)
        {
            var album = new Album
            {
                NombreAlbum = model.NombreAlbum,
                IdUsuario = model.IdUsuario,
            };
            _context.Albumes.Add(album);
            await _context.SaveChangesAsync();

            return album.IdAlbum; // Devolvemos el Id recién creado
        }

        public FotoAlbum GetFotoPorId(int idFoto)
        {

            return _context.FotoAlbumes
                .Include(f => f.MascotasEtiquetadas)
                    .ThenInclude(fe => fe.Mascota)
                .FirstOrDefault(f => f.IdFoto == idFoto);
        }
        public async Task InsertFoto(int idAlbum, string titulo, IFormFile foto, string descripcion, DateTime fecha, List<int> mascotasIds)
        {
            using var ms = new MemoryStream();
            await foto.CopyToAsync(ms);
            var bytes = ms.ToArray();

            var fotoAlbum = new FotoAlbum
            {
                IdAlbum = idAlbum,
                Titulo = titulo,
                Descripcion = descripcion,
                Fecha = fecha,
                Foto = bytes
            };

            _context.FotoAlbumes.Add(fotoAlbum);
            await _context.SaveChangesAsync();

            if (mascotasIds != null && mascotasIds.Any())
            {
                foreach (var idMascota in mascotasIds)
                {
                    var etiqueta = new FotoEtiquetaMascota
                    {
                        IdFoto = fotoAlbum.IdFoto,
                        IdMascota = idMascota
                    };
                    _context.FotoEtiquetaMascotas.Add(etiqueta);
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
