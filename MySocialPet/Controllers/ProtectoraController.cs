using Microsoft.AspNetCore.Mvc;
using MySocialPet.DAL;
using System.Threading.Tasks;

namespace MySocialPet.Controllers
{
    public class ProtectoraController : Controller
    {
        private readonly ProtectoraDAL _protectoraDal;
        private readonly UsuarioDAL _usuarioDAL;

        public ProtectoraController(ProtectoraDAL protectoraDal, UsuarioDAL usuarioDAL)
        {
            _protectoraDal = protectoraDal;
            _usuarioDAL = usuarioDAL;
        }

        // GET /Protectora
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Listado sin filtros (usa ProtectoraDAL.GetListProtectoras)
            var vm = await _protectoraDal.GetListProtectoras();
            return View(vm);
        }

        // GET /Protectora/Detalles/{id}
        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var vm = await _protectoraDal.GetProtectoraByIdAsync(id);
            if (vm == null) return NotFound();

            // 🔽 Trae hasta 12 mascotas del usuario dueño de la protectora
            var mascotas = await _usuarioDAL.GetMascotasDeUsuarioAsync(vm.IdUsuario, take: 12, soloEnAdopcion: true);

            ViewBag.Mascotas = mascotas; // sin cambiar tu modelo de vista actual
            return View(vm);
        }

    }
}
