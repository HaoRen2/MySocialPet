using Microsoft.AspNetCore.Mvc;
using MySocialPet.DAL;
using MySocialPet.Models.Albums;
using MySocialPet.Models.Foros;
using MySocialPet.Models.ViewModel.Albums;
using System.Security.Claims;

namespace MySocialPet.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly AlbumDAL _albumDAL;

        public AlbumsController(AlbumDAL albumDAL)
        {
            _albumDAL = albumDAL;
        }

        [HttpGet]
        public IActionResult ListAlbum()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var vm = new ListaAlbumViewModel
            {
                ListAlbums = _albumDAL.GetAlbumesPorUsuario(userId)
            };

            return View(vm);
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
        public IActionResult DetailsAlbum(int idAlbum)
        {
            var album = _albumDAL.GetAlbumPorId(idAlbum);
            if (album == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            ViewBag.MascotasUsuario = _albumDAL.GetListaNombreMascotasPorUsuario(userId);

            var viewModel = new DetailAlbumViewModel
            {
                DetailAlbum = album
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult EditarFoto(int idFoto)
         {
            var foto = _albumDAL.GetFotoPorId(idFoto);

            if (foto == null)
                return NotFound();

            var model = new EditarFotoViewModel
            {
                IdFoto = foto.IdFoto,
                IdAlbum = foto.IdAlbum,
                Titulo = foto.Titulo,
                Descripcion = foto.Descripcion,
                Fecha = foto.Fecha,
                NuevaFoto = foto.Foto
            };

            //ViewBag.ImagenActual = foto.Foto; // para mostrar en la vista

            return View(model);
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarFoto(EditarFotoViewModel model, IFormFile? nuevaFoto)
         {
            if (!ModelState.IsValid)
            {
                // volvemos a pasar la foto actual por ViewBag para que se muestre otra vez
                var fotoActual = _albumDAL.GetFotoPorId(model.IdFoto);
                if (fotoActual != null)
                {
                    ViewBag.ImagenActual = fotoActual.Foto;
                }
                return View("EditarFoto", model);
            }

            try
            {
                // 1️⃣ Recuperamos la foto original de la BD
                var foto = _albumDAL.GetFotoPorId(model.IdFoto);
                if (foto == null)
                    return NotFound();

                // 2️⃣ Actualizamos los campos editables
                foto.Titulo = model.Titulo;
                foto.Descripcion = model.Descripcion;
                foto.Fecha = model.Fecha;

                // 3️⃣ Si hay nueva imagen, reemplazamos los bytes
                if (nuevaFoto != null && nuevaFoto.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await nuevaFoto.CopyToAsync(ms);
                    foto.Foto = ms.ToArray();
                }

                // 4️⃣ Guardamos en BD
                await _albumDAL.UpdateFoto(foto);

                TempData["Success"] = "Foto actualizada correctamente.";
                return RedirectToAction("DetailsAlbum", new { idAlbum = foto.IdAlbum });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar la foto: " + ex.Message);

                // si falla, mostramos otra vez la foto actual
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
            if (Foto != null && Foto.Length > 0)
            {
                await _albumDAL.InsertFoto(IdAlbum, Titulo, Foto, Descripcion, Fecha, MascotasEtiquetadasIds);
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
        public async Task<IActionResult> DeleteFoto(int idAlbum)
        {
            try
            {
                await _albumDAL.DeleteAlbum(idAlbum);
                TempData["Success"] = "Álbum eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar el álbum: " + ex.Message;
            }

            return RedirectToAction("ListAlbum");
    }
    


}
}