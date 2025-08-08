using Microsoft.AspNetCore.Mvc;
using MySocialPet.DAL;
using MySocialPet.Models.Albums;
using MySocialPet.Models.ViewModel.Albums;
using MySocialPet.Models.ViewModel.Mascotas;
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
            ListaAlbumViewModel vm = new ListaAlbumViewModel();

            vm.ListAlbums = _albumDAL.GetAlbumesPorUsuario(userId);

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
                // Guardar el álbum
                await _albumDAL.InsertAlbum(model);
                
                return RedirectToAction("ListAlbum");
            }
            return View(model);
        }

        public IActionResult DetailsAlbum(int idAlbum)
        {

            return View();
        }
        /* [HttpGet]
         public JsonResult GetNombresMascotasporUser()
         {
             int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

             var mascotasNombre = _albumDAL.GetListaNombreMascotasPorUsuario(userId);

             return Json(mascotasNombre);
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> CreateAlbum(CreateAlbumViewModel model)
         {
             if (ModelState.IsValid)
             {
                 var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                 model.IdUsuario = int.Parse(userId);
                 // Guardar el álbum
                 await _albumDAL.InsertAlbum(model);
                 // Guardar las fotos del álbum
                 if (model.Fotos != null && model.Fotos.Count > 0)
                 {
                     foreach (var foto in model.Fotos)
                     {
                         await _albumDAL.InsertFoto(foto, model.IdAlbum);
                     }
                 }
                 return RedirectToAction("ListAlbum");
             }
             else 
             {
                 // Si el modelo no es válido, volvemos a cargar los datos necesarios para la vista
                 var viewModel = new CreateAlbumViewModel
                 {
                     IdUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                     Fotos = model.Fotos, // Mantener las fotos subidas si hay errores de validación
                     NombreMascota = model.NombreMascota,
                     IdMascota = model.IdMascota
                 };
                 // Cargar las mascotas del usuario para el dropdown
                 viewModel.Mascotas = _albumDAL.GetListaNombreMascotasPorUsuario(viewModel.IdUsuario);
                 return View(viewModel);
             }*/



    }
    
}