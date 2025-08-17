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
        public IActionResult DetailsFoto(int id)//esto ahora sera para EditarFoto
        {
            var fotoVM = _albumDAL.GetFotoPorId(id);
            if (fotoVM == null)
                return NotFound();

            var viewModel = new DetailsFotoViewModel
            {
                Foto = fotoVM
            };


            return View("DetailsFoto", viewModel);
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
        public async Task<IActionResult> DeleteFoto(int id)
        {
            // Primero, obtenemos la foto para saber a qué álbum pertenece y así poder redirigir correctamente.
            var foto = _albumDAL.GetFotoPorId(id);
            if (foto == null)
            {
                return NotFound();
            }

            // Guardamos el ID del álbum antes de que la foto sea eliminada.
            var idAlbum = foto.IdAlbum;

            try
            {
                await _albumDAL.DeleteFoto(id);
                TempData["Success"] = "Foto eliminada correctamente."; // Mensaje de éxito opcional
            }
            catch (Exception ex)
            {
                // Es una buena práctica registrar el error en un log
                // Log.Error(ex, "Error al eliminar la foto");
                TempData["Error"] = "Error al eliminar la foto: " + ex.Message;
            }

            // Redirigimos de vuelta a la página de detalles del álbum.
            return RedirectToAction("DetailsAlbum", new { idAlbum = idAlbum });
        }



    }
}