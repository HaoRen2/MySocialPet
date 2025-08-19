using Microsoft.AspNetCore.Mvc;
using MySocialPet.Models.ViewModel.Mascotas;
using System.Security.Claims;

namespace MySocialPet.ViewsComponents
{
    public class MascotaSelectorViewComponent : ViewComponent
    {
        private readonly AppDbContexto _context;

        public MascotaSelectorViewComponent(AppDbContexto context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int mascotaIdActual, string controlador, string accion)
        {
            var usuarioId = ObtenerUsuarioIdActual(); 

            var mascotas = _context.Mascotas
                .Where(m => m.IdUsuario == usuarioId)
                .ToList();

            var model = new MascotaSelectorViewModel
            {
                Mascotas = mascotas,
                MascotaIdActual = mascotaIdActual,
                Controlador = controlador,
                Accion = accion
            };

            return View(model);
        }

        private int ObtenerUsuarioIdActual()
        {
            return int.Parse(UserClaimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
