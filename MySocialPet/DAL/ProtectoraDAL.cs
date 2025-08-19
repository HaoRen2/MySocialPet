using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySocialPet.Models;
using MySocialPet.Models.ViewModel.Protectora;

namespace MySocialPet.DAL
{
    public class ProtectoraDAL
    {
        private readonly AppDbContexto _context;

        public ProtectoraDAL(AppDbContexto context)
        {
            _context = context;
        }

        // Listado (sin filtros/paginación explícitos)
        public async Task<ProtectoraListViewModel> GetListProtectoras()
        {
            // Agregados por usuario (Perros/Gatos) en UNA sola subconsulta
            var countsByUser = _context.Mascotas
                .Where(m => m.Raza != null && m.Raza.Especie != null)
                .GroupBy(m => m.IdUsuario)
                .Select(g => new
                {
                    IdUsuario = g.Key,
                    NumPerros = g.Sum(m => m.Raza.Especie.Nombre == "Perro" ? 1 : 0),
                    NumGatos = g.Sum(m => m.Raza.Especie.Nombre == "Gato" ? 1 : 0)
                });

            var total = await _context.Protectoras.CountAsync();

            var items = await _context.Protectoras
                .AsNoTracking()
                // LEFT JOIN con los agregados por usuario
                .GroupJoin(
                    countsByUser,
                    p => p.IdUsuario,
                    c => c.IdUsuario,
                    (p, cg) => new { p, c = cg.FirstOrDefault() }
                )
                .Select(x => new ProtectoraViewModel
                {
                    // --- Usuario (solo los campos que necesitas en el VM) ---
                    Email = x.p.Usuario != null ? x.p.Usuario.Email : "",
                    AvatarFoto = x.p.Usuario != null ? x.p.Usuario.AvatarFoto : null,
                    Telefono = x.p.Usuario != null ? x.p.Usuario.Telefono : null,
                    Direccion = x.p.Usuario != null ? x.p.Usuario.Direccion : null,
                    Ciudad = x.p.Usuario != null ? x.p.Usuario.Ciudad : null,
                    Provincia = x.p.Usuario != null ? x.p.Usuario.Provincia : null,
                    CodigoPostal = x.p.Usuario != null ? x.p.Usuario.CodigoPostal : null,

                    // --- Protectora ---
                    IdUsuario = x.p.IdUsuario,
                    IdProtectora = x.p.IdProtectora,
                    Nombre = x.p.Nombre,
                    Web = x.p.Web,
                    Descripcion = x.p.Descripcion,
                    Introduccion = x.p.Introduccion,

                    // --- Derivados ---
                    NumPerros = x.c != null ? x.c.NumPerros : 0,
                    NumGatos = x.c != null ? x.c.NumGatos : 0
                })
                .ToListAsync();

            return new ProtectoraListViewModel
            {
                Protectoras = items,
                Total = total,
                Page = 1,
                PageSize = items.Count // sin paginación explícita
            };
        }

        // Detalle por Id (incluye datos de Usuario y conteos)
        public async Task<ProtectoraViewModel?> GetProtectoraByIdAsync(int idProtectora)
        {
            // Subconsulta de agregados por usuario
            var countsByUser = _context.Mascotas
                .Where(m => m.Raza != null && m.Raza.Especie != null)
                .GroupBy(m => m.IdUsuario)
                .Select(g => new
                {
                    IdUsuario = g.Key,
                    NumPerros = g.Sum(m => m.Raza.Especie.Nombre == "Perro" ? 1 : 0),
                    NumGatos = g.Sum(m => m.Raza.Especie.Nombre == "Gato" ? 1 : 0)
                });

            return await _context.Protectoras
                .AsNoTracking()
                .Where(p => p.IdProtectora == idProtectora)
                .GroupJoin(
                    countsByUser,
                    p => p.IdUsuario,
                    c => c.IdUsuario,
                    (p, cg) => new { p, c = cg.FirstOrDefault() }
                )
                .Select(x => new ProtectoraViewModel
                {
                    // --- Usuario ---
                    Email = x.p.Usuario != null ? x.p.Usuario.Email : "",
                    AvatarFoto = x.p.Usuario != null ? x.p.Usuario.AvatarFoto : null,
                    Telefono = x.p.Usuario != null ? x.p.Usuario.Telefono : null,
                    Direccion = x.p.Usuario != null ? x.p.Usuario.Direccion : null,
                    Ciudad = x.p.Usuario != null ? x.p.Usuario.Ciudad : null,
                    Provincia = x.p.Usuario != null ? x.p.Usuario.Provincia : null,
                    CodigoPostal = x.p.Usuario != null ? x.p.Usuario.CodigoPostal : null,

                    // --- Protectora ---
                    IdUsuario = x.p.IdUsuario,
                    IdProtectora = x.p.IdProtectora,
                    Nombre = x.p.Nombre,
                    Web = x.p.Web,
                    Descripcion = x.p.Descripcion,
                    Introduccion = x.p.Introduccion,

                    // --- Derivados ---
                    NumPerros = x.c != null ? x.c.NumPerros : 0,
                    NumGatos = x.c != null ? x.c.NumGatos : 0
                })
                .FirstOrDefaultAsync();
        }
    }
}
