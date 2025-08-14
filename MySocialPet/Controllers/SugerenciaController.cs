using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySocialPet.DAL;
using MySocialPet.Models.ViewModel.Sugerencias;
using static MySocialPet.Models.ViewModel.Sugerencias.SugerenciaViewModel;

namespace MySocialPet.Controllers
{
    public class SugerenciaController : Controller
    {
        private readonly SugerenciaDAL _sugerenciaDAL;
        private readonly AppDbContexto _context;


        public SugerenciaController(SugerenciaDAL sugerenciaDAL, AppDbContexto context)
        {
            _sugerenciaDAL = sugerenciaDAL;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int? IdEspecie, int? IdCategoria, int? IdRaza)
        {

            var especiesList = new SelectList(_context.Especies.ToList(), "IdEspecie", "Nombre");

            var vm = new SugerenciaViewModel
            {
                EspeciesSelectList = especiesList,
                CategoriasSelectList = new SelectList(Enumerable.Empty<SelectListItem>()),
                RazasSelectList = new SelectList(Enumerable.Empty<SelectListItem>()),

                IdEspecie = IdEspecie,
                IdCategoria = IdCategoria,
                IdRaza = IdRaza
            };


            var query = _context.Sugerencias
                .Include(s => s.EspeciesSugerencia).ThenInclude(es => es.Especie)
                .Include(s => s.CategoriaSugerencias).ThenInclude(cs => cs.Categoria)
                .Include(s => s.RazasSugerencia).ThenInclude(rs => rs.Raza)
                .AsQueryable();

            if (IdEspecie.HasValue)
            {

                var categorias = _context.Categorias
                               .Where(c => c.IdEspecie == IdEspecie.Value)
                                .ToList();

                vm.CategoriasSelectList = new SelectList(categorias, "IdCategoria", "NombreCategoria", IdCategoria);

                if (IdCategoria.HasValue) 
                {
                    var razas = _context.Razas
                  .Where(r => r.IdCategoria == IdCategoria.Value).ToList();

                    vm.RazasSelectList = new SelectList(razas, "IdRaza", "NombreRaza", IdRaza);
                }

                query = query.Where(s =>
                s.EspeciesSugerencia.Any(es => es.IdEspecie == IdEspecie) ||
                s.RazasSugerencia.Any(rs => rs.Raza.IdEspecie == IdEspecie));
            }

            if (IdCategoria.HasValue)
            {
                query = query.Where(s =>
                    s.CategoriaSugerencias.Any(cs => cs.IdCategoria == IdCategoria) ||
                    s.RazasSugerencia.Any(rs => rs.Raza.IdCategoria == IdCategoria)
                );
            }

            if (IdRaza.HasValue)
            {
                query = query.Where(s =>
                    s.RazasSugerencia.Any(rs => rs.IdRaza == IdRaza)
                );
            }

            vm.Sugerencias = query.OrderBy(q => q.Titulo).ToList();

            return View(vm);
        }
        [HttpGet]
        public IActionResult GetRazasPorCategoria(int idCategoria)
        {
            var razas = _context.Razas
                .Where(r => r.IdCategoria == idCategoria)
                .Select(r => new { id = r.IdRaza, nombre = r.NombreRaza })
                .ToList();

            return Json(razas);
        }

        [HttpGet]
        public IActionResult GetCategoriasPorEspecie(int idEspecie)
        {
            var categorias = _context.Categorias
                .Where(c => c.IdEspecie == idEspecie)
                .Select(c => new { id = c.IdCategoria, nombre = c.NombreCategoria })
                .ToList();

            return Json(categorias);
        }
    }
}
