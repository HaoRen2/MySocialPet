using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySocialPet.DAL;
using MySocialPet.Models.Mascotas;
using MySocialPet.Models.Salud;
using MySocialPet.Models.ViewModel.Salud;

namespace MySocialPet.Controllers
{
    public class SaludController : Controller
    {

        private readonly AppDbContexto _context;

        public SaludController(AppDbContexto context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GestionarSaludRegistro(int id)
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
        public async Task<IActionResult> GestionarSaludRegistro(SaludRegistroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var salud = new SaludRegistro
            {
                Fecha = model.Fecha,
                PesoKg = model.PesoKg,
                LongitudCm = model.LongitudCm,
                BCS = model.BCS ?? 0,
                Notas = model.Notas,
                IdMascota = model.IdMascota
            };

            _context.SaludRegistros.Add(salud);
            await _context.SaveChangesAsync();

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> CalendarioEventos(int id)
        {
            var mascota = await _context.Mascotas
                    .Include(m => m.Eventos)
                    .Include(m => m.Raza)
                        .ThenInclude(r => r.Especie)
                    .FirstOrDefaultAsync(m => m.IdMascota == id);

            if (mascota == null)
                return NotFound();

            var viewModel = new CalendarioEventosViewModel
            {
                IdMascota = mascota.IdMascota,
                NombreMascota = mascota.Nombre,
                Eventos = mascota.Eventos.Select(e => new EventoViewModel
                {
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearEvento(EventoViewModel datosFormulario)
        {
            if (ModelState.IsValid)
            {
                var nuevoEvento = new Evento
                {
                    Titulo = datosFormulario.Titulo,
                    FechaHora = datosFormulario.FechaHora,
                    TipoEvento = datosFormulario.TipoEvento,
                    Notas = datosFormulario.Notas,
                    IdMascota = datosFormulario.IdMascota
                };

                _context.Eventos.Add(nuevoEvento);
                await _context.SaveChangesAsync();

                return RedirectToAction("CalendarioEventos", new { id = datosFormulario.IdMascota });
            }

            return RedirectToAction("CalendarioEventos", new { id = datosFormulario.IdMascota });
        }

    }
}
