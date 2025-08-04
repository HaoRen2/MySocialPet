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

        [HttpGet]
        public IActionResult Details(int id)
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
            vm.DetailDiscusion = discusiones;

            if (vm.DetailDiscusion != null)
            {
                ViewBag.Error = "Foro no encontrado.";
            }

            return View(vm);
        }
    }
}
