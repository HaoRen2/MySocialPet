using Microsoft.AspNetCore.Mvc;
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
                    .ThenInclude(f => f.MascotasEtiquetadas)
                        .ThenInclude(fe => fe.Mascota)
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

        public async Task UpdateAlbumName(int idAlbum, string nuevoNombre)
        {
            var album = await _context.Albumes.FindAsync(idAlbum);
            if (album == null)
            {
                throw new Exception("El álbum no existe.");
            }

            album.NombreAlbum = nuevoNombre;
            _context.Albumes.Update(album);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAlbum(int idAlbum)
        {
            // Buscamos el álbum con sus fotos y etiquetas
            var album = await _context.Albumes
                .Include(a => a.Fotos)
                    .ThenInclude(f => f.MascotasEtiquetadas)
                .FirstOrDefaultAsync(a => a.IdAlbum == idAlbum);

            if (album == null)
            {
                throw new Exception("El álbum no existe.");
    }

            // 1️⃣ Eliminar etiquetas de cada foto
            foreach (var foto in album.Fotos)
            {
                if (foto.MascotasEtiquetadas != null && foto.MascotasEtiquetadas.Any())
                {
                    _context.FotoEtiquetaMascotas.RemoveRange(foto.MascotasEtiquetadas);
                }
            }

            // 2️⃣ Eliminar fotos del álbum (si hay)
            if (album.Fotos != null && album.Fotos.Any())
            {
                _context.FotoAlbumes.RemoveRange(album.Fotos);
            }

            // 3️⃣ Eliminar álbum
            _context.Albumes.Remove(album);

            await _context.SaveChangesAsync();
}

        public FotoAlbum GetFotoPorId(int idFoto)
        {
            return _context.FotoAlbumes
                .Include(f => f.MascotasEtiquetadas)
                    .ThenInclude(fe => fe.Mascota)
                .FirstOrDefault(f => f.IdFoto == idFoto);
        }

        public async Task UpdateFoto(FotoAlbum foto)
        {
            _context.FotoAlbumes.Update(foto);
            await _context.SaveChangesAsync();
        }


        public async Task InsertFoto(int idAlbum, string titulo, byte[] fotoBytes, string descripcion, DateTime fecha, List<int> mascotasIds)
        {
            var fotoAlbum = new FotoAlbum
            {
                IdAlbum = idAlbum,
                Titulo = titulo,
                Descripcion = descripcion,
                Fecha = fecha,
                Foto = fotoBytes // Se usa directamente el array de bytes
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
        public async Task DeleteFoto(int idFoto)
        {
            // Primero, eliminamos las etiquetas de mascota asociadas a la foto.
            var etiquetas = await _context.FotoEtiquetaMascotas
                .Where(e => e.IdFoto == idFoto)
                .ToListAsync();

            if (etiquetas.Any())
            {
                _context.FotoEtiquetaMascotas.RemoveRange(etiquetas);
            }

            // Luego, buscamos y eliminamos la foto.
            var foto = await _context.FotoAlbumes.FindAsync(idFoto);
            if (foto != null)
            {
                _context.FotoAlbumes.Remove(foto);
                await _context.SaveChangesAsync(); // Guardamos todos los cambios en la base de datos.
            }
        }
    }
}