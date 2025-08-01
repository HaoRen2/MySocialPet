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
           

                var usuario = new Usuario
                {
                    Username = model.Username,
                    Email = model.Email
                };

                // IMPROVEMENT: Asumimos que CreateUsuario ahora devuelve el usuario creado o null si falla
                var usuarioCreado = _usuarioDAL.CreateUsuario(usuario, model.Password);

                if (usuarioCreado != null)
                {
                    // FIX: Iniciar sesión inmediatamente después de registrarse
                    await LoginConClaim(usuarioCreado);
                    return RedirectToAction("Index", "Home");
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
                Usuario usuario = _usuarioDAL.GetUsuarioLogin(model.Username, model.Password);
                if (usuario != null)
                {
                    await LoginConClaim(usuario);
                    return RedirectToAction("Index", "Home");
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

        // FIX: El método debe devolver Task en lugar de void
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