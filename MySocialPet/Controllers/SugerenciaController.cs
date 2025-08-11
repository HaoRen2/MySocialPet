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
            var categoriasList = new SelectList(_context.Categorias.ToList(), "IdCategoria", "NombreCategoria"); // Asegúrate de que tu propiedad se llame "Nombre" o ajústalo.

            var razasQuery = _context.Razas.AsQueryable();
            if (IdEspecie.HasValue)
            {
                razasQuery = razasQuery.Where(r => r.IdEspecie == IdEspecie.Value);
            }
            var razasList = new SelectList(razasQuery.ToList(), "IdRaza", "NombreRaza"); // Ajusta "Nombre" si es necesario.


            var vm = new SugerenciaViewModel
            {
                EspeciesSelectList = especiesList,
                CategoriasSelectList = categoriasList,
                RazasSelectList = razasList,

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
                query = query.Where(s => s.EspeciesSugerencia.Any(es => es.IdEspecie == IdEspecie));
            }

            if (IdCategoria.HasValue)
            {
                query = query.Where(s => s.CategoriaSugerencias.Any(cs => cs.IdCategoria == IdCategoria));
            }

            if (IdRaza.HasValue)
            {
                query = query.Where(s => s.RazasSugerencia.Any(rs => rs.IdRaza == IdRaza));
            }

            vm.Sugerencias = query.ToList();

            return View(vm);
        }


    }
}
