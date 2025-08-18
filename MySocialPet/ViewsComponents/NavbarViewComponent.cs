using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySocialPet.DAL;
using MySocialPet.Models.Autenticacion;
using MySocialPet.Models.Mascotas;
using System.Security.Claims;

namespace MySocialPet.ViewsComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly AppDbContexto _context;
        private readonly UsuarioDAL _usuarioDAL;

        public NavbarViewComponent(AppDbContexto context, UsuarioDAL usuarioDAL)
        {
            _context = context;
            _usuarioDAL = usuarioDAL;
        }
        public async Task<IViewComponentResult> InvokeAsync(int? idMascota)
        {
            var user = HttpContext?.User;
            var isAuthenticated = user?.Identity?.IsAuthenticated == true;
            var username = isAuthenticated ? user.Identity.Name : null;

            // Id del usuario desde el claim
            var idStr = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Fallback del avatar
            string avatarUrl = Url.Content("~/src/DefaultAvatar/AvatarPerro.png");

            // Si está autenticado y el Id es válido, cargamos avatar y mascota
            int idUsuario;
            if (isAuthenticated && int.TryParse(idStr, out idUsuario))
            {
                // Avatar desde DAL (GetAvatarAsync devuelve byte[])
                var bytes = await _usuarioDAL.GetAvatarAsync(idUsuario);
                if (bytes != null && bytes.Length > 0)
                {
                    avatarUrl = "data:image/png;base64," + Convert.ToBase64String(bytes);
                }

                // Mascota por defecto si no viene
                if (!idMascota.HasValue || idMascota.Value == 0)
                {
                    idMascota = await _context.Mascotas
                        .Where(m => m.IdUsuario == idUsuario)
                        .Select(m => m.IdMascota)
                        .FirstOrDefaultAsync();
                }
            }

            // Pasamos datos a la vista
            ViewBag.IsAuthenticated = isAuthenticated;
            ViewBag.Username = username;
            ViewBag.FotoAvatar = avatarUrl;

            return View(idMascota ?? 0);
        }
    }
}
