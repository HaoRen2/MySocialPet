using Microsoft.AspNetCore.Mvc;
using MySocialPet.Models.Autenticacion;
using MySocialPet.Models.Mascotas;
using System.Security.Claims;

namespace MySocialPet.ViewsComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly AppDbContexto _context;

        public NavbarViewComponent(AppDbContexto context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke(int? idMascota)
        {
            var isAuthenticated = User.Identity.IsAuthenticated;
            var username = isAuthenticated ? User.Identity.Name : null;
            var id = UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            ViewBag.IsAuthenticated = isAuthenticated;
            ViewBag.Username = username;

            if (!idMascota.HasValue || idMascota == 0)
            {
                idMascota = _context.Mascotas
                    .Where(m => m.IdUsuario.ToString() == id)
                    .Select(m => m.IdMascota)
                    .FirstOrDefault();
            }

            return View(idMascota ?? 0);
        }
    }
}
