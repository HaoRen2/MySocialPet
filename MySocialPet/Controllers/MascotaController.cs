using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySocialPet.DAL;
using MySocialPet.Models.Mascotas;
using MySocialPet.Models.ViewModel;
using MySocialPet.Models.ViewModel.Mascotas;
using System.Security.Claims;

namespace MySocialPet.Controllers
{
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
                Id = a.IdMascota,
                Nombre = a.Nombre,
                Foto = a.Foto,
                Genero = a.Genero,
                Nacimiento = a.Nacimiento,
                PesoKg = a.PesoKg,
                LongitudCm = a.LongitudCm,
                BCS = a.BCS,
                Esterilizada = a.Esterilizada
            }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var mascota = _mascotaDAL.GetMascota(id);

            if (mascota == null)
            {
                return NotFound();
            }

            var viewModel = new DetailMascotaViewModel
            {
                DetailMascota = mascota
            };

            return View(viewModel);
        }


        [HttpGet]
        public IActionResult CreateMascota()
        {
            var viewModel = _mascotaDAL.GetEspecie();

            return View(viewModel);
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
                return RedirectToAction("Index", "Home");
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
            double ratioIdeal;

            if (especie == "perro")
            {
                if (tamano.Contains("pequeño") || tamano.Contains("toy")) ratioIdeal = 0.15;
                else if (tamano.Contains("grande")) ratioIdeal = 0.55;
                else if (tamano.Contains("gigante")) ratioIdeal = 0.80;
                else ratioIdeal = 0.35; // Mediano
            }
            else if (especie == "gato")
            {
                if (tamano.Contains("grande") || tamano.Contains("gigante")) ratioIdeal = 0.16;
                else ratioIdeal = 0.11; // Mediano o Común
            }
            else
            {
                return Json(new { bcsValor = 0, bcsTexto = "No aplicable" });
            }

            double ratioActual = pesoKg / longitudCm;

            // CALCULAMOS VALOR Y TEXTO POR SEPARADO
            int bcsValor;
            string bcsTexto;

            if (ratioActual < ratioIdeal * 0.85)
            {
                bcsValor = 1;
                bcsTexto = "1 - Muy Delgado";
            }
            else if (ratioActual < ratioIdeal * 0.95)
            {
                bcsValor = 2;
                bcsTexto = "2 - Delgado";
            }
            else if (ratioActual <= ratioIdeal * 1.15)
            {
                bcsValor = 3;
                bcsTexto = "3 - Ideal";
            }
            else if (ratioActual <= ratioIdeal * 1.30)
            {
                bcsValor = 4;
                bcsTexto = "4 - Sobrepeso";
            }
            else
            {
                bcsValor = 5;
                bcsTexto = "5 - Obeso";
            }

            // Devolvemos un objeto con ambas propiedades
            return Json(new { bcsValor, bcsTexto });
        }

    }
}
