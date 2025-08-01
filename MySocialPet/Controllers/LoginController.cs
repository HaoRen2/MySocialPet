using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MySocialPet.DAL;
using MySocialPet.Models.Autenticacion;
using MySocialPet.Models.ViewModel.Autenticacion;
using System.Security.Claims;

namespace MySocialPet.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioDAL _usuarioDAL;

        public LoginController(UsuarioDAL usuarioDAL)
        {
            _usuarioDAL = usuarioDAL;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existente = _usuarioDAL.GetUsuarioByEmail(model.Email);
                if (existente != null)
                {
                    ModelState.AddModelError("Email", "Ya existe una cuenta con este correo electrónico.");
                    return View(model);
                }

                var usuario = new Usuario
                {
                    Username = model.Username,
                    Email = model.Email
                };

                try
                {
                    var usuarioCreado = _usuarioDAL.CreateUsuario(usuario, model.Password);
                    if (usuarioCreado != null)
                    {
                        await LoginConClaim(usuarioCreado);
                        return RedirectToAction("ListaMascota", "Mascota");
                    }
                    ModelState.AddModelError("", "No se ha podido completar el registro.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error interno: {ex.Message}");
                }

                ModelState.AddModelError("", "No se ha podido completar el registro. Inténtalo de nuevo.");
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = _usuarioDAL.GetUsuarioLogin(model.Email, model.Password);
                if (usuario != null)
                {
                    await LoginConClaim(usuario);
                    return RedirectToAction("ListaMascota", "Mascota");
                }
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private async Task LoginConClaim(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Username),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}