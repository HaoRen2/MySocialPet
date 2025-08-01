using Microsoft.AspNetCore.Mvc;
using MySocialPet.Models.ViewModel.Mascotas;

namespace MySocialPet.Controllers
{
    public class MascotaController : Controller
    {
        private readonly AppDbContexto _context;

        public MascotaController(AppDbContexto context)
        {
            _context = context;
        }

        public IActionResult ListaMascota()
        {
            var viewModel = _context.Mascotas
                .Select(a => new ListaMascotaViewModel
                {
                    Id = a.IdMascota,
                    Nombre = a.Nombre,
                    Nacimiento = a.Nacimiento,
                    PesoKg = a.PesoKg,
                    LongitudCm = a.LongitudCm,
                    Genero = a.Genero,
                    Foto = a.Foto,
                    BCS = a.BCS,
                    Esterilizada = a.Esterilizada,
                    IdRaza = a.IdRaza,
                    NombreRaza = a.Raza.NombreRaza
                })
                .ToList();

            return View(viewModel);
        }


    }
}
