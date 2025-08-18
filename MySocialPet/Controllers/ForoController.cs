using Microsoft.AspNetCore.Mvc;
using MySocialPet.DAL;
using MySocialPet.Models.Foros;
using MySocialPet.Models.ViewModel.Foros;
using Newtonsoft.Json;
using System.Security.Claims;

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

            List<Discusion> tendencias = _foroDAL.GetTrendingDiscusionsAsync(null).Result;

            vm.Tendencias = tendencias;
            vm.Foros = _foroDAL.GetForos();

            return View(vm);
        }

        [HttpGet("Foro/Hilos/{slug}")]
        public IActionResult Hilos(string slug)
        {
            ListaDiscusionesViewModel vm = new ListaDiscusionesViewModel();


            Foro? foro = _foroDAL.GetForoBySlug(slug);
            if (foro == null)
            {
                ViewBag.Error = "Foro no encontrado.";
                return RedirectToAction("Index");
            }

            List<Discusion> discusiones = _foroDAL.GetDiscusionesPorForo(foro.IdForo);
            List<Discusion> tendencias = _foroDAL.GetTrendingDiscusionsAsync(null).Result;

            vm.Foro = foro;
            vm.Discusions = discusiones;
            vm.Tendencias = tendencias;

            return View(vm);
        }

        [HttpGet]
        public IActionResult NuevoHilo(int foroId)
        {
            var foro = _foroDAL.GetForoById(foroId);
            if (foro == null)
            {
                TempData["Error"] = "Foro no encontrado.";
                return RedirectToAction("Index");
            }

            var vm = new NuevoHiloViewModel
            {
                IdForo = foroId
            };

            return View(vm);
        }

        [HttpGet("Foro/Hilos/{slug}/{foroId}/{discId}")]
        public IActionResult HiloDetails(string slug, int foroId, int discId, string? ordenarPor)
        {
            // buscar foro por slug
            Foro? foro = _foroDAL.GetForoBySlug(slug);
            if (foro == null) return NotFound();

            // buscar discusión por id y comprobar que pertenece al foro
            Discusion? disc = _foroDAL.GetDiscusionById(discId);
            if (disc == null || disc.IdForo != foro.IdForo) return NotFound();

            List<Discusion> tendencias = _foroDAL.GetTrendingDiscusionsAsync(null).Result;

            // Lógica para ordenar los mensajes
            if (disc.Mensajes != null)
            {
                switch (ordenarPor)
                {
                    case "antiguos":
                        disc.Mensajes = disc.Mensajes.OrderBy(m => m.FechaEnvio).ToList();
                        break;
                    case "recientes":
                    default: // Por defecto, ordena por más recientes
                        disc.Mensajes = disc.Mensajes.OrderByDescending(m => m.FechaEnvio).ToList();
                        break;
                }
            }

            var vm = new DetailDiscusionViewModel
            {
                IdForo = foro.IdForo,
                IdDiscusion = discId,
                Slug = slug,
                Discusion = disc,
                Tendencias = tendencias
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NuevoHilo(NuevoHiloViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var discusion = new Discusion
            {
                IdForo = vm.IdForo,
                Titulo = vm.Titulo,
                Descripcion = vm.Descripcion,
                FechaCreacion = DateTime.Now,
                EstaFinalizado = false,
                IdUsuarioCreador = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
            };

            _foroDAL.CrearHilo(discusion);

            TempData["Success"] = "Hilo creado correctamente.";
            return RedirectToAction("Hilos", new { id = vm.IdForo });
        }

        [HttpGet("Foro/Hilos/{slug}/{foroId}/{discId}/Mensaje")]
        public IActionResult EnviarMensaje(string slug, int foroId, int discId, int? mensajePadreId)
        {
            var foro = _foroDAL.GetForoById(foroId);
            var disc = _foroDAL.GetDiscusionById(discId);

            if (foro == null || disc == null || disc.IdForo != foro.IdForo)
                return NotFound();

            var vm = new EnviarMensajeViewModel
            {
                IdForo = foroId,
                IdDiscusion = discId,
                IdMensajePadre = mensajePadreId
            };

            return View(vm);
        }

        [HttpPost("Foro/Hilos/{slug}/{foroId}/{discId}/Mensaje")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnviarMensaje(EnviarMensajeViewModel vm, string slug)
        {
            if (!ModelState.IsValid)
                return View(vm);

            byte[] fotoData = null;
            if (vm.Imagen != null && vm.Imagen.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await vm.Imagen.CopyToAsync(memoryStream);
                    fotoData = memoryStream.ToArray();
                }
            }

            var mensaje = new Mensaje
            {
                IdDiscusion = vm.IdDiscusion,
                IdMensajePadre = vm.IdMensajePadre,
                ContenidoMensaje = vm.Contenido,
                Imagen = fotoData,
                FechaEnvio = DateTime.Now,
                IdUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
            };

            await _foroDAL.CrearMensaje(mensaje);

            TempData["Success"] = vm.IdMensajePadre.HasValue
                ? "Respuesta añadida correctamente."
                : "Mensaje añadido correctamente.";

            return RedirectToAction("HiloDetails", new { slug = slug, foroId = vm.IdForo, discId = vm.IdDiscusion });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarMensaje(int id)
        {
            // Validar que el usuario actual es el creador del mensaje
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var mensaje = await _foroDAL.GetMensajeById(id);

            if (mensaje == null || mensaje.IdUsuario != userId)
            {
                return Forbid(); // O una vista de error, si lo prefieres
            }

            await _foroDAL.EliminarMensaje(id);

            // Puedes redireccionar al mismo hilo después de eliminar el mensaje
            return RedirectToAction("HiloDetails", "Foro", new { slug = mensaje.Discusion.Foro.Slug, foroId = mensaje.Discusion.IdForo, discId = mensaje.IdDiscusion });
        }

    }
}
