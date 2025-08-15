using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySocialPet.DAL;
using MySocialPet.Models.Mascotas;
using MySocialPet.Models.Salud;
using MySocialPet.Models.ViewModel.Salud;

namespace MySocialPet.Controllers
{
    [Authorize]
    public class SaludController : Controller
    {

        private readonly AppDbContexto _context;
        private readonly SaludDAL _saludDAL;
        private readonly MascotaDAL _mascotaDAL;


        public SaludController(AppDbContexto context, SaludDAL saludDAL, MascotaDAL mascotaDAL)
        {
            _context = context;
            _saludDAL = saludDAL;
            _mascotaDAL = mascotaDAL;
        }

        [HttpGet]
        public async Task<IActionResult> SaludRegistro(int id)
        {
            var mascota = await _context.Mascotas
                .Include(m => m.Raza)
                .FirstOrDefaultAsync(m => m.IdMascota == id);

            if (mascota == null)
                return NotFound();

            var model = new SaludRegistroViewModel
            {
                IdMascota = mascota.IdMascota,
                NombreMascota = mascota.Nombre,
                PesoKg = mascota.PesoKg,
                LongitudCm = mascota.LongitudCm
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaludRegistro(SaludRegistroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var salud = new SaludRegistro
            {
                Fecha = model.Fecha,
                PesoKg = model.PesoKg,
                LongitudCm = model.LongitudCm,
                BCS = model.BCS ?? 0,
                IdMascota = model.IdMascota
            };

            _context.SaludRegistros.Add(salud);
            await _context.SaveChangesAsync();

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> CalendarioEventos(int id)
        {
            var mascota = _saludDAL.GetEventosMascota(id);

            if (mascota == null)
                return NotFound();

            var viewModel = new CalendarioEventosViewModel
            {
                IdMascota = mascota.IdMascota,
                NombreMascota = mascota.Nombre,
                Eventos = mascota.Eventos.Select(e => new EventoViewModel
                {
                    IdEvento = e.IdEvento,
                    Titulo = e.Titulo,
                    FechaHora = e.FechaHora,
                    Color = e.Color,
                    Notas = e.Notas,
                    IdMascota = e.IdMascota
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvento(EventoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var nuevoEvento = new Evento
                {
                    Titulo = model.Titulo,
                    FechaHora = model.FechaHora,
                    Color = model.Color,
                    Notas = model.Notas,
                    IdMascota = model.IdMascota
                };

               _saludDAL.InsertMascota(nuevoEvento);

                return RedirectToAction("CalendarioEventos", new { id = model.IdMascota });
            }

            return RedirectToAction("CalendarioEventos", new { id = model.IdMascota });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvento(EventoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var eventoExistente = await _context.Eventos
           .FirstOrDefaultAsync(e => e.IdEvento == model.IdEvento);

                if (eventoExistente != null)
                {
                    eventoExistente.Titulo = model.Titulo;
                    eventoExistente.FechaHora = model.FechaHora;
                    eventoExistente.Color = model.Color;
                    eventoExistente.Notas = model.Notas;
                    eventoExistente.IdMascota = model.IdMascota;

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("CalendarioEventos", new { id = model.IdMascota });
            }

            return RedirectToAction("CalendarioEventos", new { id = model.IdMascota });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
                return NotFound();

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Vacunas(int id)
        {
            var mascota = await _context.Mascotas
           .Include(m => m.Raza)
           .ThenInclude(r => r.Especie)
           .FirstOrDefaultAsync(m => m.IdMascota == id);

            if (mascota == null)
                return NotFound();

            var idEspecie = mascota.Raza.IdEspecie;

            var listaVacunas = await _context.ListaVacunas
                .Include(lv => lv.TipoVacuna)
                .Where(lv => lv.IdEspecie == idEspecie)
                .ToListAsync();

            var vacunasRegistradas = await _context.VacunaRegistros
                .Where(v => v.IdMascota == id)
                .Include(v => v.TipoVacuna)
                .ToListAsync();

            var vacunasVM = listaVacunas.Select(lv =>
            {
                var registro = vacunasRegistradas.FirstOrDefault(v => v.IdTipoVacuna == lv.IdTipoVacuna);

                return new VacunaDetalleViewModel
                {
                    NombreVacuna = lv.TipoVacuna.Nombre,
                    EdadRecomendada = lv.EdadRecomendada,
                    EsRefuerzo = lv.EsRefuerzo,
                    Descripcion = lv.Descripcion,
                    Aplicada = registro != null,
                    FechaAplicacion = registro?.Fecha,
                    Esencial = lv.Esencial,
                    Notas = lv.Notas,
                };
            }).ToList();

            var viewModel = new VacunaMascotaViewModel
            {
                IdMascota = mascota.IdMascota,
                NombreMascota = mascota.Nombre,
                NombreEspecie = mascota.Raza.Especie.Nombre,
                Vacunas = vacunasVM
            };

            var vacunasRegistradasIds = vacunasRegistradas.Select(v => v.IdTipoVacuna).ToList();

            ViewBag.Vacunas = _context.TipoVacunas
                .Where(v => !vacunasRegistradasIds.Contains(v.IdTipoVacuna))
                .OrderBy(v => v.Nombre)
                .Select(v => new SelectListItem
                {
                    Value = v.IdTipoVacuna.ToString(),
                    Text = v.Nombre
                })
                .ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarVacuna(int IdMascota, int IdTipoVacuna, DateTime Fecha)
        {
            var registro = new VacunaRegistro
            {
                IdMascota = IdMascota,
                IdTipoVacuna = IdTipoVacuna,
                Fecha = Fecha,
            };

            _context.VacunaRegistros.Add(registro);
            await _context.SaveChangesAsync();

            return RedirectToAction("Vacunas","Salud", new { id = IdMascota });
        }

        [HttpGet]
        public IActionResult ObtenerEvolucion(int idMascota)
        {
            var registros = _context.SaludRegistros
                .Where(r => r.IdMascota == idMascota)
                .OrderBy(r => r.Fecha)
                .Select(r => new
                {
                    fecha = r.Fecha.ToString("yyyy-MM-dd"),
                    peso = r.PesoKg,
                    bcs = r.BCS
                })
                .ToList();

            return Json(registros);
        }

        // Calculacion de caloria recomendada
        [HttpGet]
        public IActionResult CalcularCalorias(int idMascota)
        {
            var mascota = _context.Mascotas
                .Include(m => m.Raza)
                .ThenInclude(r => r.Especie)
                .FirstOrDefault(m => m.IdMascota == idMascota);

            if (mascota == null)
                return Json(new { calorias = 0 });

            string especie = mascota.Raza.Especie.Nombre;
            bool esterilizada = mascota.Esterilizada;
            double peso = (double)mascota.PesoKg;

            double rer = 70 * Math.Pow(peso, 0.75);

            double factor;
            if (especie.ToLower() == "perro")
                factor = esterilizada ? 1.6 : 1.8;
            else if (especie.ToLower() == "gato")
                factor = esterilizada ? 1.2 : 1.4;
            else factor = 1;

            double calorias = Math.Round(rer * factor, 0);

            return Json(new { calorias });
        }

        [HttpGet]
        public async Task<IActionResult> EstimarIndiceCorporal(double pesoKg, double longitudLomoCm, int idRaza)
        {
            if (pesoKg <= 0 || longitudLomoCm <= 0 || idRaza <= 0)
            {
                return Json(new { exito = false, mensaje = "Los datos introducidos no son válidos." });
            }

            // --- Obtención de datos de la raza ---
            // ASUMIMOS que ahora tu clase 'Raza' tiene una propiedad: public double? RatioIdeal { get; set; }
            var raza =  _mascotaDAL.GetRazaDeMascota(idRaza);

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

            if (desviacion <= -35) { valorEstimado = 1; textoOrientativo = "1/9 - Posiblemente muy delgado"; }
            else if (desviacion <= -20) { valorEstimado = 3; textoOrientativo = "3/9 - Posiblemente delgado"; }
            else if (desviacion <= -10) { valorEstimado = 4; textoOrientativo = "4/9 - Posiblemente en el límite inferior del ideal"; }
            else if (desviacion <= 10) { valorEstimado = 5; textoOrientativo = "5/9 - Aparentemente en un rango de peso ideal"; }
            else if (desviacion <= 20) { valorEstimado = 6; textoOrientativo = "6/9 - Posiblemente en el límite superior del ideal"; }
            else if (desviacion <= 35) { valorEstimado = 7; textoOrientativo = "7/9 - Posiblemente con sobrepeso"; }
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
