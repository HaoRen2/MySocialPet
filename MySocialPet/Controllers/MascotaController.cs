using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySocialPet.DAL;
using MySocialPet.Models.Mascotas;
using MySocialPet.Models.Salud;
using MySocialPet.Models.ViewModel;
using MySocialPet.Models.ViewModel.Mascotas;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MySocialPet.Controllers
{
    [Authorize]
    public class MascotaController : Controller
    {
        private readonly MascotaDAL _mascotaDAL;
        public MascotaController(MascotaDAL mascotaDAL)
        {
            _mascotaDAL = mascotaDAL;
        }

        [HttpGet]
        public IActionResult ListaMascota()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var mascotas = _mascotaDAL.GetListaPorUsuario(userId);

            var viewModel = mascotas.Select(a => new ListaMascotaViewModel
            {
                IdMascota = a.IdMascota,
                Nombre = a.Nombre,
                Foto = a.Foto,
                Genero = a.Genero,
                Nacimiento = a.Nacimiento,
                PesoKg = a.PesoKg,
                LongitudCm = a.LongitudCm,
                BCS = a.BCS,
                Esterilizada = a.Esterilizada,
                NombreRaza = a.Raza.NombreRaza,
                Evento = a.Eventos.Where(e => e.FechaHora > DateTime.Now).
                OrderBy(e => (e.FechaHora - DateTime.Now).TotalSeconds).FirstOrDefault()
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var mascota = _mascotaDAL.GetMascotaById(id);

            if (mascota == null)
            {
                return NotFound();
            }

            var viewModel = new DetailMascotaViewModel
            {
                DetailMascota = mascota,
                Notas = mascota.Notas.OrderByDescending(n => n.IdNota).ToList(),
                Evento = mascota.Eventos.Where(e => e.FechaHora > DateTime.Now).
                OrderBy(e => (e.FechaHora - DateTime.Now).TotalSeconds).FirstOrDefault()
            };

            if (mascota.IdUsuario.ToString() == User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                return View(viewModel);
            else
                return RedirectToAction("ListaMascota");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarNota(int id, string descripcion)
        {
            if (!string.IsNullOrWhiteSpace(descripcion))
            {
                var nota = new Nota
                {
                    IdMascota = id,
                    Descripcion = descripcion
                };

                _mascotaDAL.InsertNotas(nota);
            }

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarNota(int id)
        {
            var nota = _mascotaDAL.GetNotabyId(id);
            if (nota != null)
            {
                await _mascotaDAL.DeleteNota(nota);
                return RedirectToAction("Details", new { id = nota.IdMascota });
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNota(Nota nota)
        {
            await _mascotaDAL.UpdateNota(nota);
            return RedirectToAction("Details", new { id = nota.IdMascota });
        }



        [HttpGet]
        public IActionResult CreateMascota()
        {
            var viewModel = _mascotaDAL.GetEspecie();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult EditMascota(int id)
        {
            var mascota = _mascotaDAL.GetMascotaById(id);
            if (mascota == null)
                return NotFound();

            var especieViewModel = _mascotaDAL.GetEspecie();
            var razasViewModel = _mascotaDAL.GetRazaPorEspecie(mascota.Raza.IdEspecie);

            var model = new CrearMascotaViewModel
            {
                Id = mascota.IdMascota,
                Nombre = mascota.Nombre,
                Nacimiento = mascota.Nacimiento,
                PesoKg = mascota.PesoKg,
                LongitudCm = mascota.LongitudCm,
                Genero = mascota.Genero,
                BCS = mascota.BCS,
                Esterilizada = mascota.Esterilizada,
                IdRaza = mascota.IdRaza,
                IdEspecie = mascota.Raza.IdEspecie,
                Especies = especieViewModel.Especies,
                Razas = razasViewModel
            };

            return View("EditMascota", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMascota(int id)
        {
            var mascota = _mascotaDAL.GetMascotaById(id);
            if (mascota == null)
                return NotFound();

            try
            {
                await _mascotaDAL.DeleteMascota(id);
                return RedirectToAction("ListaMascota");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar la mascota: " + ex.Message;
                return RedirectToAction("ListaMascota");
            }
        }

        [HttpGet]
        public JsonResult GetRazasPorEspecie(int id)
        {
            var razas = _mascotaDAL.GetRazaPorEspecie(id);

            return Json(razas);
        }

        [HttpGet]
        public IActionResult GetInfoRaza(int id)
        {
            var raza = _mascotaDAL.DatosRaza(id);

            if (raza == null)
            {
                return NotFound();
            }

            var resultado = new
            {
                nombreRaza = raza.NombreRaza,
                informacion = raza.Informacion,
                tamanyo = raza.Tamanyo,
                foto = string.IsNullOrEmpty(raza.Foto) ? "/images/default-raza.png" : raza.Foto,
                categoria = new
                {
                    nombre = raza.Categoria?.NombreCategoria
                }
            };
            return Json(resultado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMascota(CrearMascotaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = _mascotaDAL.GetEspecie();
                model.Especies = viewModel.Especies;
                model.Razas = viewModel.Razas;
                return View(model);
            }

            byte[] fotoData = null;
            if (model.Foto != null && model.Foto.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.Foto.CopyToAsync(memoryStream);
                    fotoData = memoryStream.ToArray();
                }
            }

            var mascota = new Mascota
            {
                Nombre = model.Nombre,
                Nacimiento = model.Nacimiento,
                PesoKg = model.PesoKg,
                LongitudCm = model.LongitudCm,
                Genero = model.Genero,
                BCS = model.BCS,
                Esterilizada = model.Esterilizada,
                IdRaza = model.IdRaza,
                Foto = fotoData,
                IdUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                EstadoAdopcion = "No está en adopción"
            };

            try
            {
                await _mascotaDAL.InsertMascota(mascota);
                return RedirectToAction("ListaMascota", "Mascota");
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " | Inner Exception: " + ex.InnerException.Message;
                }
                ModelState.AddModelError(string.Empty, "Error al guardar la mascota: " + ex.Message);

                var viewModel = _mascotaDAL.GetEspecie();
                model.Especies = viewModel.Especies;
                model.Razas = viewModel.Razas;

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMascota(CrearMascotaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var especieData = _mascotaDAL.GetEspecie();
                model.Especies = especieData.Especies;
                model.Razas = _mascotaDAL.GetRazaPorEspecie(model.IdRaza);
                return View("EditMascota", model);
            }

            int id = (int)model.Id;

            var mascota = _mascotaDAL.GetMascotaById(id);
            if (mascota == null)
                return NotFound();

            mascota.Nombre = model.Nombre;
            mascota.Nacimiento = model.Nacimiento;
            mascota.PesoKg = model.PesoKg;
            mascota.LongitudCm = model.LongitudCm;
            mascota.Genero = model.Genero;
            mascota.BCS = model.BCS;
            mascota.Esterilizada = model.Esterilizada;
            mascota.IdRaza = model.IdRaza;

            if (model.Foto != null && model.Foto.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await model.Foto.CopyToAsync(memoryStream);
                mascota.Foto = memoryStream.ToArray();
            }

            try
            {
                await _mascotaDAL.UpdateMascota(mascota);
                return RedirectToAction("ListaMascota");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error al actualizar la mascota: " + ex.Message);
                var especieData = _mascotaDAL.GetEspecie();
                model.Especies = especieData.Especies;
                model.Razas = _mascotaDAL.GetRazaPorEspecie(model.IdRaza);
                return View("EditMascota", model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EstimarIndiceCorporal(double pesoKg, double longitudLomoCm, int idRaza)
        {
            if (pesoKg <= 0 || longitudLomoCm <= 0 || idRaza <= 0)
            {
                return Json(new { exito = false, mensaje = "Los datos introducidos no son válidos." });
            }

            var raza = _mascotaDAL.GetRazaDeMascota(idRaza);

            if (raza == null || raza.Especie == null)
            {
                return Json(new { exito = false, mensaje = "Raza no encontrada o datos incompletos." });
            }

            string especie = raza.Especie.Nombre.ToLower();
            string tamano = raza.Tamanyo?.ToLower() ?? "";
            int ratioIdeal = 0;
            // --- CÁLCULO DEL RATIO IDEAL (Lógica mejorada) ---
            // double? ratioIdeal = null;

            // 1. Prioridad: Usar el ratio específico de la raza si existe en la BD.
            /*
            if (raza.RatioIdeal != null && raza.RatioIdeal > 0)
            {
                ratioIdeal = raza.RatioIdeal;
            }
            */
            // 2. Fallback: Si no hay ratio específico, usar la lógica general por tamaño.
            if (especie == "perro")
            {
                if (tamano.Contains("toy") || tamano.Contains("pequeño")) ratioIdeal = 28;
                else if (tamano.Contains("mediano")) ratioIdeal = 30;
                else if (tamano.Contains("grande")) ratioIdeal = 32;
                else if (tamano.Contains("gigante")) ratioIdeal = 34;
                else ratioIdeal = 30; // Default para perros
            }
            else if (especie == "gato")
            {
                // Para gatos, es mejor tener ratios por raza. Si no, usamos un default genérico
                // pero somos conscientes de su alta imprecisión.
                ratioIdeal = 30; // Default para gatos (con sus limitaciones ya discutidas)
            }

            if (ratioIdeal <= 0)
            {
                return Json(new { exito = false, mensaje = "No se pudo determinar un ratio ideal para esta especie o raza." });
            }

            // --- CÁLCULO DEL ÍNDICE ACTUAL Y LA DESVIACIÓN ---
            double longitudMetros = longitudLomoCm / 100.0;
            double ratioActual = pesoKg / Math.Pow(longitudMetros, 2);
            double desviacion = (ratioActual - ratioIdeal) / ratioIdeal * 100;

            // --- MAPEO A UNA ESCALA ORIENTATIVA ---
            int valorEstimado;
            string textoOrientativo;

            if (desviacion <= -40) { valorEstimado = 1; textoOrientativo = "1/9 - Posiblemente muy emaciado"; }
            else if (desviacion <= -30) { valorEstimado = 2; textoOrientativo = "2/9 - Posiblemente muy delgado"; }
            else if (desviacion <= -20) { valorEstimado = 3; textoOrientativo = "3/9 - Posiblemente delgado"; }
            else if (desviacion <= -10) { valorEstimado = 4; textoOrientativo = "4/9 - Posiblemente en el límite inferior del ideal"; }
            else if (desviacion <= 10) { valorEstimado = 5; textoOrientativo = "5/9 - Aparentemente en un rango de peso ideal"; }
            else if (desviacion <= 20) { valorEstimado = 6; textoOrientativo = "6/9 - Posiblemente en el límite superior del ideal"; }
            else if (desviacion <= 30) { valorEstimado = 7; textoOrientativo = "7/9 - Posiblemente con sobrepeso"; }
            else if (desviacion <= 40) { valorEstimado = 8; textoOrientativo = "8/9 - Posiblemente con sobrepeso severa"; }
            else { valorEstimado = 9; textoOrientativo = "9/9 - Posiblemente con obesidad"; }

            // --- MENSAJE DE ADVERTENCIA (EL CAMBIO MÁS IMPORTANTE) ---
            return Json(new
            {
                valorEstimado,
                textoOrientativo,
            });
        }
    }
}
