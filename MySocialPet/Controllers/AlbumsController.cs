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
            ListaAlbumViewModel vm = new ListaAlbumViewModel();

            vm.ListAlbums = _albumDAL.GetAlbumesPorUsuario(userId);
            /*foreach (var album in vm.ListAlbums)
            {
                // Obtener la foto más reciente de cada álbum
                album.FotoReciente = _albumDAL.GetFotoRecinete(album.IdAlbum.ToString());
            }*/

            return View(vm);
        }
    }
}
