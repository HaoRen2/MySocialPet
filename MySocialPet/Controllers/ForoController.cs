using Microsoft.AspNetCore.Mvc;
using MySocialPet.DAL;
using MySocialPet.Models.Foros;
using MySocialPet.Models.ViewModel.Foros;
using Newtonsoft.Json;

namespace MySocialPet.Controllers
{
    public class ForoController : Controller
    {
        private readonly ForoDAL _foroDAL;
        public ForoController(ForoDAL foroDAL)
        {
            _foroDAL = foroDAL;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IndexForoViewModel vm = new IndexForoViewModel();

            vm.Foros = _foroDAL.GetForos();

            return View(vm);
        }

        [HttpPost]
        public IActionResult Hilos(int id)
        {
            ListaDiscusionesViewModel vm = new ListaDiscusionesViewModel();

            Foro? foro = _foroDAL.GetForoById(id);
            List<Discusion> discusiones = _foroDAL.GetDiscusionesPorForo(id);

            if (foro == null)
            {
                ViewBag.Error = "Foro no encontrado.";
                return RedirectToAction("Index");
            }

            vm.Foro = foro;
            vm.Discusions = discusiones;

            return View(vm);
        }

        [HttpPost]
        public IActionResult HiloDetails(int foroId, int discId)
        {
            DetailDiscusionViewModel vm = new DetailDiscusionViewModel();

            vm.IdForo = foroId;

            if (_foroDAL.GetForoById(foroId) != null)
            {
                TempData["Hilo"] = JsonConvert.SerializeObject(vm);
            }
            else
            {
                ViewBag.NoAnimal = "No se ha encontrado ningún hilo con esa identificación.";
            }

            return RedirectToAction("Index", "Hilo");
        }
    }
}
