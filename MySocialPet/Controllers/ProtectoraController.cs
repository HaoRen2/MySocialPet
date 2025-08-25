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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarLogo(int id, IFormFile logo)
        {
            if (logo == null || logo.Length == 0)
                return BadRequest("Archivo vacío.");

            // 1) Verificar que exista la protectora
            var protectora = await _protectoraDal.GetProtectoraByIdAsync(id);
            if (protectora == null) return NotFound();

            // 2) Leer bytes del archivo
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                await logo.CopyToAsync(ms);
                bytes = ms.ToArray();
            }

            // 3) Aquí podrías validar/transformar la imagen si quisieras (resize, formato, etc.)
            //    Por ahora, subimos tal cual:
            byte[] fotoToUpdate = bytes;

            // 4) Actualizar el "avatar" del usuario dueño de la protectora (tu DAL existente)
            await _usuarioDAL.UpdateAvatarAsync(protectora.IdUsuario, fotoToUpdate);

            // 5) Data URL para refrescar al instante en la vista
            var contentType = string.IsNullOrWhiteSpace(logo.ContentType) ? "image/png" : logo.ContentType;
            var dataUrl = $"data:{contentType};base64,{Convert.ToBase64String(fotoToUpdate)}";

            return Json(new
            {
                success = true,
                previewDataUrl = dataUrl,
                message = "Logo actualizado"
            });
        }



    }
}
