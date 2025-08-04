using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySocialPet.Models.Mascotas;
using MySocialPet.Models.ViewModel.Mascotas;

namespace MySocialPet.DAL
{
    public class MascotaDAL
    {
        private readonly AppDbContexto _context;

        public MascotaDAL(AppDbContexto context)
        {
            _context = context;
        }


        public async Task InsertMascota(Mascota mascota)
        {
            _context.Mascotas.Add(mascota);
            await _context.SaveChangesAsync();
        }

        public List<Especie> GetEspecies()
        {
            return _context.Especies.ToList();
        }

        public Raza? DatosRaza(int id)
        {
            return _context.Razas
                .Include(r => r.Categoria) 
                .FirstOrDefault(r => r.IdRaza == id);
        }

        public List<Mascota> GetListaPorUsuario(string userId)
        {
            return _context.Mascotas
                .Where(a => a.IdUsuario.ToString() == userId)
                .Include(a => a.Raza)
                .ToList();
        }

        public Mascota GetMascota(int id)
        {
            return _context.Mascotas.Include(m => m.Raza)
                .FirstOrDefault(m => m.IdMascota == id);
        }

        public Raza GetRazaDeMascota(int idRaza)
        {
            return _context.Razas.Include(r => r.Especie).FirstOrDefault(r => r.IdRaza == idRaza);
        }

        public List<SelectListItem> GetRazaPorEspecie(int id)
        {
            return _context.Razas
                .Where(r => r.IdEspecie == id)
                .Select(r => new SelectListItem
                {
                    Value = r.IdRaza.ToString(),
                    Text = r.NombreRaza.ToString()
                }).ToList();
        }

        public CrearMascotaViewModel GetEspecie()
        {
            return new CrearMascotaViewModel
            {
                Especies = _context.Especies
                 .Select(e => new SelectListItem { Value = e.IdEspecie.ToString(), Text = e.Nombre })
                 .ToList(),

                Razas = new List<SelectListItem>()
            };
        }

    }
}
