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
                Evento = a.Eventos .Where(e => e.FechaHora > DateTime.Now).
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
                Especies = especieViewModel.Especies,
                Razas = _mascotaDAL.GetRazaPorEspecie(mascota.Raza.IdEspecie)
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
                _mascotaDAL.UpdateMascota(mascota);
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
        public async Task<IActionResult> CalcularBCS(double pesoKg, double longitudCm, int idRaza)
        {
            if (pesoKg <= 0 || longitudCm <= 0 || idRaza <= 0)
            {
                return Json(new { bcsValor = 0, bcsTexto = "" });
            }

            var raza = _mascotaDAL.GetRazaDeMascota(idRaza);

            if (raza == null || raza.Especie == null || string.IsNullOrEmpty(raza.Tamanyo))
            {
                return Json(new { bcsValor = 0, bcsTexto = "Raza no encontrada" });
            }

            string especie = raza.Especie.Nombre.ToLower();
            string tamano = raza.Tamanyo.ToLower();

            // Calcular ratio tipo IMC: peso en kg / (longitud en metros)^2
            double longitudMetros = longitudCm / 100.0;
            double ratioActual = pesoKg / Math.Pow(longitudMetros, 2);

            // Valores de IMC aproximados para ideal según especie/tamaño
            double ratioIdeal;
            if (especie == "perro")
            {
                if (tamano.Contains("toy") || tamano.Contains("pequeño")) ratioIdeal = 28;
                else if (tamano.Contains("mediano")) ratioIdeal = 30;
                else if (tamano.Contains("grande")) ratioIdeal = 32;
                else if (tamano.Contains("gigante")) ratioIdeal = 34;
                else ratioIdeal = 30;
            }
            else if (especie == "gato")
            {
                ratioIdeal = 30; // Gatos adultos estándar
            }
            else
            {
                return Json(new { bcsValor = 0, bcsTexto = "No aplicable" });
            }

            // Calcular desviación en porcentaje
            double desviacion = (ratioActual - ratioIdeal) / ratioIdeal * 100;

            // Escala BCS estándar (1 a 9)
            int bcsValor;
            string bcsTexto;

            if (desviacion <= -40) { bcsValor = 1; bcsTexto = "1 - Emaciado"; }
            else if (desviacion <= -30) { bcsValor = 2; bcsTexto = "2 - Muy Delgado"; }
            else if (desviacion <= -20) { bcsValor = 3; bcsTexto = "3 - Delgado"; }
            else if (desviacion <= -10) { bcsValor = 4; bcsTexto = "4 - Ligeramente Delgado"; }
            else if (desviacion <= 10) { bcsValor = 5; bcsTexto = "5 - Ideal"; }
            else if (desviacion <= 20) { bcsValor = 6; bcsTexto = "6 - Ligeramente Sobrepeso"; }
            else if (desviacion <= 30) { bcsValor = 7; bcsTexto = "7 - Sobrepeso"; }
            else if (desviacion <= 40) { bcsValor = 8; bcsTexto = "8 - Obeso"; }
            else { bcsValor = 9; bcsTexto = "9 - Obesidad Severa"; }

            return Json(new { bcsValor, bcsTexto });
        }
    }
}
