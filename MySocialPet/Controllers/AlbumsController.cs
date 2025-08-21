using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySocialPet.DAL;
using MySocialPet.Models.Albums;
using MySocialPet.Models.Foros;
using MySocialPet.Models.ViewModel.Albums;
using MySocialPet.Models.ViewModel.Mascotas;
using MySocialPet.Tools;
using System.Security.Claims;

namespace MySocialPet.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly AlbumDAL _albumDAL;
        private readonly AppDbContexto _context;


        public AlbumsController(AlbumDAL albumDAL, AppDbContexto context)
        {
            _albumDAL = albumDAL;
            _context = context;
        }


        [HttpGet]
        public IActionResult ListAlbum(int pagina = 1)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            int tamanoPagina = 5;

            var todosLosAlbumes = _albumDAL.GetAlbumesPorUsuario(userId);

            var totalAlbumes = todosLosAlbumes.Count();

            var albumesDeLaPagina = todosLosAlbumes
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList(); 

            var viewModel = new ListaAlbumViewModel
            {
                ListAlbums = albumesDeLaPagina,

                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling(totalAlbumes / (double)tamanoPagina)
            };

            if (viewModel.TotalPaginas == 0)
            {
                viewModel.TotalPaginas = 1;
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CrearAlbum()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearAlbum(CrearAlbumViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                model.IdUsuario = int.Parse(userId);
                
                // Guardar el álbum y obtener su Id
                var nuevoAlbumId = await _albumDAL.InsertAlbum(model);

                // Redirigir a DetallesAlbum con el Id recién creado
                return RedirectToAction("DetailsAlbum", new { idAlbum = nuevoAlbumId });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult DetailsAlbum(int idAlbum, int pagina = 1)
        {
            var album = _albumDAL.GetAlbumPorId(idAlbum);
            if (album == null)
            {
                return NotFound();
            }

            int tamanoPagina = 12;

            var todasLasFotos = album.Fotos;
            var totalFotos = todasLasFotos.Count();

            var fotosDeLaPagina = todasLasFotos
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            ViewBag.MascotasUsuario = _albumDAL.GetListaNombreMascotasPorUsuario(userId);

            var viewModel = new DetailAlbumViewModel
            {
                IdAlbum = album.IdAlbum,
                NombreAlbum = album.NombreAlbum,
                TotalFotos = totalFotos,
                FotosDeLaPagina = fotosDeLaPagina,
                PaginaActual = pagina,
                TotalPaginas = (int)Math.Ceiling(totalFotos / (double)tamanoPagina)
            };

            if (viewModel.TotalPaginas == 0)
            {
                viewModel.TotalPaginas = 1;
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult EditarFoto(int idFoto)
        {
            var foto = _albumDAL.GetFotoPorId(idFoto);
            if (foto == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var mascotasUsuario = _albumDAL.GetListaNombreMascotasPorUsuario(userId);

            var model = new EditarFotoViewModel
            {
                IdFoto = foto.IdFoto,
                IdAlbum = foto.IdAlbum,
                Titulo = foto.Titulo,
                Descripcion = foto.Descripcion,
                Fecha = foto.Fecha,
                NuevaFoto = foto.Foto,
                MascotasEtiquetadasIds = foto.MascotasEtiquetadas?.Select(m => m.IdMascota).ToList(),
                MascotasDisponibles = mascotasUsuario // esto devuelve List<SelectListItem> con Value=Id y Text=Nombre
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarFoto(EditarFotoViewModel model, IFormFile? nuevaFoto)
        {
            if (!ModelState.IsValid)
            {
                var fotoActual = _albumDAL.GetFotoPorId(model.IdFoto);
                if (fotoActual != null)
                {
                    ViewBag.ImagenActual = fotoActual.Foto;
                }
                return View("EditarFoto", model);
            }

            try
            {
                var foto = _albumDAL.GetFotoPorId(model.IdFoto);
                if (foto == null)
                    return NotFound();

                foto.Titulo = model.Titulo;
                foto.Descripcion = model.Descripcion;
                foto.Fecha = model.Fecha;

                if (nuevaFoto != null && nuevaFoto.Length > 0)
                {
                    if (nuevaFoto.ContentType == "image/gif")
                    {
                        var compressedGif = await GifCompressor.CompressToUnderAsync(
                          nuevaFoto,
                          maxBytes: 5 * 1024 * 1024,
                          maxWidth: 640
                        );
                        foto.Foto = compressedGif.Data;
                    }
                    else if (nuevaFoto.ContentType == "image/jpeg" || nuevaFoto.ContentType == "image/png")
                    {
                        var compressed = await ImageCompressor.CompressToUnderAsync(
                          nuevaFoto,
                          maxBytes: 5 * 1024 * 1024,
                          maxWidth: 1920,
                          keepTransparency: nuevaFoto.ContentType == "image/png"
                        );
                        foto.Foto = compressed.Data;
                    }
                    else
                    {
                        ModelState.AddModelError("NuevaFoto", "Formato no soportado. Solo se permiten JPG, PNG o GIF.");
                        ViewBag.ImagenActual = foto.Foto;
                        return View("EditarFoto", model);
                    }
                }

                // ✅ Actualizar las etiquetas
                foto.MascotasEtiquetadas.Clear(); // limpiamos las antiguas
                if (model.MascotasEtiquetadasIds != null && model.MascotasEtiquetadasIds.Any())
                {
                    foreach (var idMascota in model.MascotasEtiquetadasIds.Where(x => x.HasValue))
                    {
                        foto.MascotasEtiquetadas.Add(new FotoEtiquetaMascota
                        {
                            IdFoto = foto.IdFoto,
                            IdMascota = idMascota.Value
                        });
                    }
                }

                await _albumDAL.UpdateFoto(foto);

                TempData["Success"] = "Foto actualizada correctamente.";
                return RedirectToAction("DetailsAlbum", new { idAlbum = foto.IdAlbum });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar la foto: " + ex.Message);
                var fotoActual = _albumDAL.GetFotoPorId(model.IdFoto);
                if (fotoActual != null)
                {
                    ViewBag.ImagenActual = fotoActual.Foto;
                }
                return View("EditarFoto", model);
            }
        }

        [HttpGet]
        public IActionResult EditarAlbum(int idAlbum)
             {
            var album = _albumDAL.GetAlbumPorId(idAlbum);
            if (album == null)
                 {
                return NotFound(); // Retorna un error 404 si el álbum no se encuentra
            }

            var model = new EditarAlbumViewModel
                     {
                IdAlbum = album.IdAlbum,
                NombreAlbum = album.NombreAlbum
            };

            return View(model);
                     }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarAlbum(EditarAlbumViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Vuelve a mostrar la vista si la validación falla
                 }

            try
            {
                await _albumDAL.UpdateAlbumName(model.IdAlbum, model.NombreAlbum);
                TempData["Success"] = "Álbum actualizado correctamente.";
                 return RedirectToAction("ListAlbum");
             }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar el álbum: " + ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertarFoto(int IdAlbum, string Titulo, IFormFile Foto, string Descripcion, DateTime Fecha, List<int> MascotasEtiquetadasIds)
        {
            byte[] fotoData = null;

            if (Foto != null && Foto.Length > 0)
            {
                if (Foto.ContentType == "image/gif")
                {
                    var compressedGif = await GifCompressor.CompressToUnderAsync(
                        Foto,
                        maxBytes: 5 * 1024 * 1024, // 5MB para GIFs
                        maxWidth: 640
                    );
                    fotoData = compressedGif.Data;
                }
                else if (Foto.ContentType == "image/jpeg" || Foto.ContentType == "image/png")
                {
                    var compressed = await ImageCompressor.CompressToUnderAsync(
                        Foto,
                        maxBytes: 5 * 1024 * 1024, // 5MB para imágenes
                        maxWidth: 1920,
                        keepTransparency: Foto.ContentType == "image/png"
                    );
                    fotoData = compressed.Data;
                }
                else
                {
                    TempData["Error"] = "Formato de imagen no soportado. Solo se permiten JPG, PNG o GIF.";
                    return RedirectToAction("DetailsAlbum", new { idAlbum = IdAlbum });
                }

                // Llamada al método del DAL con los bytes de la foto
                await _albumDAL.InsertFoto(IdAlbum, Titulo, fotoData, Descripcion, Fecha, MascotasEtiquetadasIds);
            }
            else
            {
                TempData["Error"] = "No se ha seleccionado ninguna foto.";
            }

            return RedirectToAction("DetailsAlbum", new { idAlbum = IdAlbum });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAlbum(int idAlbum)
        {
            try
             {
                // ✅ Llamamos al DAL para que elimine el álbum y sus fotos asociadas
                await _albumDAL.DeleteAlbum(idAlbum);

                TempData["Success"] = "Álbum eliminado correctamente junto con sus fotos.";
            }
            catch (Exception ex)
                 {
                // Registrar el error (log recomendado)
                TempData["Error"] = "Error al eliminar el álbum: " + ex.Message;
            }

            // Redirigir de vuelta a la lista de álbumes
            return RedirectToAction("ListAlbum");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFoto(int idFoto)
        {
            try
            {
                // Primero, eliminamos las etiquetas de mascota asociadas a la foto.
                var etiquetas = await _context.FotoEtiquetaMascotas
                    .Where(e => e.IdFoto == idFoto)
                    .ToListAsync();

                if (etiquetas.Any())
                {
                    _context.FotoEtiquetaMascotas.RemoveRange(etiquetas);
                }

                var foto = await _context.FotoAlbumes.FindAsync(idFoto);

                if (foto == null)
                {
                    return NotFound(new { message = "La foto que intentas eliminar no existe." });
                }

                _context.FotoAlbumes.Remove(foto);
                await _context.SaveChangesAsync(); 

                return Ok(new { message = "Foto eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error en el servidor al eliminar la foto." });
            }
        }


    }
}