using Microsoft.AspNetCore.Mvc;
using MySocialPet.DAL;
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
    }
}
