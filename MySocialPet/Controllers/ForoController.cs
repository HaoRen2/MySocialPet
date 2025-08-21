using Microsoft.AspNetCore.Mvc;
using MySocialPet.DAL;
using MySocialPet.Models.Foros;
using MySocialPet.Models.ViewModel.Foros;
using MySocialPet.Tools;
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
        public async Task<IActionResult> Index()
        {
            IndexForoViewModel vm = new IndexForoViewModel();
            vm.Tendencias = await _foroDAL.GetTrendingDiscusionsAsync(null);
            vm.Foros = await _foroDAL.GetForosAsync();

            return View(vm);
        }

        [HttpGet("Foro/Hilos/{slug}")]
        public async Task<IActionResult> Hilos(string slug)
        {
            ListaDiscusionesViewModel vm = new ListaDiscusionesViewModel();

            Foro? foro = await _foroDAL.GetForoBySlugAsync(slug);
            if (foro == null)
            {
                ViewBag.Error = "Foro no encontrado.";
                return RedirectToAction("Index");
            }

            vm.Discusions = await _foroDAL.GetDiscusionesPorForoAsync(foro.IdForo);
            vm.Tendencias = await _foroDAL.GetTrendingDiscusionsAsync(null);

            vm.Foro = foro;
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> NuevoHilo(int foroId)
        {
            var foro = await _foroDAL.GetForoByIdAsync(foroId);
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
        public async Task<IActionResult> HiloDetails(
            string slug, int foroId, int discId,
            string? ordenarPor, int page = 1, int pageSize = 10)
        {
            Foro? foro = await _foroDAL.GetForoBySlugAsync(slug);
            if (foro == null) return NotFound();

            // primero traemos la discusión sin mensajes
            DiscusionMensajes? disc = await _foroDAL.GetDiscusionByIdAsync(discId);
            if (disc == null || disc.IdForo != foro.IdForo) return NotFound();

            var (mensajes, total) = await _foroDAL.GetMensajesByDiscusionAsync(discId, ordenarPor, page, pageSize);

            disc.Mensajes = mensajes;

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalMensajes = total;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            ViewBag.OrdenarPor = ordenarPor ?? "recientes";

            var vm = new DetailDiscusionViewModel
            {
                IdForo = foro.IdForo,
                IdDiscusion = discId,
                Slug = slug,
                DiscusionMensajes = disc,
                Tendencias = await _foroDAL.GetTrendingDiscusionsAsync(null)
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NuevoHilo(NuevoHiloViewModel vm)
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

            await _foroDAL.CrearHiloAsync(discusion);

            TempData["Success"] = "Hilo creado correctamente.";
            return RedirectToAction("Hilos", new { id = vm.IdForo });
        }

        [HttpGet("Foro/Hilos/{slug}/{foroId}/{discId}/Mensaje")]
        public async Task<IActionResult> EnviarMensaje(string slug, int foroId, int discId, int? mensajePadreId)
        {
            var foro = await _foroDAL.GetForoByIdAsync(foroId);
            var disc = await _foroDAL.GetDiscusionByIdAsync(discId);

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
                if (vm.Imagen.ContentType == "image/gif")
                {
                    var compressedGif = await GifCompressor.CompressToUnderAsync(
                        vm.Imagen,
                        maxBytes: 2 * 1024 * 1024, // límite 2 MB
                        maxWidth: 640             // opcional: escalar para gifs
                    );
                    fotoData = compressedGif.Data;
                }
                else if (vm.Imagen.ContentType == "image/jpeg" || vm.Imagen.ContentType == "image/png")
                {
                    var result = await ImageCompressor.CompressToUnderAsync(
                        vm.Imagen,
                        maxBytes: 2 * 1024 * 1024,
                        maxWidth: 1920,
                        keepTransparency: vm.Imagen.ContentType == "image/png"
                    );
                    fotoData = result.Data;
                }
                else
                {
                    ModelState.AddModelError("Imagen", "Formato no soportado. Solo se permiten JPG, PNG o GIF.");
                    return View(vm);
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
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var mensaje = await _foroDAL.GetMensajeById(id);

            if (mensaje == null || mensaje.IdUsuario != userId)
            {
                return Forbid();
            }

            await _foroDAL.EliminarMensaje(id);

            return RedirectToAction("HiloDetails", "Foro", new { slug = mensaje.Discusion.Foro.Slug, foroId = mensaje.Discusion.IdForo, discId = mensaje.IdDiscusion });
        }

        [HttpGet("Foro/Mensaje/Imagen/{id}")]
        public async Task<IActionResult> ImagenMensaje(int id)
        {
            var mensaje = await _foroDAL.GetMensajeByIdAsync(id);
            if (mensaje == null || mensaje.Imagen == null)
                return NotFound();

            // Detectar formato de la imagen según la cabecera mágica
            string contentType = "image/jpeg";
            var img = mensaje.Imagen;

            if (img.Length > 4)
            {
                // JPG
                if (img[0] == 0xFF && img[1] == 0xD8)
                    contentType = "image/jpeg";
                // PNG
                else if (img[0] == 0x89 && img[1] == 0x50 && img[2] == 0x4E && img[3] == 0x47)
                    contentType = "image/png";
                // GIF
                else if (img[0] == 0x47 && img[1] == 0x49 && img[2] == 0x46)
                    contentType = "image/gif";
            }

            return File(mensaje.Imagen, contentType);
        }


    }
}