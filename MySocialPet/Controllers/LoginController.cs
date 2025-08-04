using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MySocialPet.DAL;
using MySocialPet.Models.Autenticacion;
using MySocialPet.Models.ViewModel.Autenticacion;
using MySocialPet.Tools;
using System.Security.Claims;
using System.Diagnostics;


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

        [HttpGet]
        public IActionResult RecuperarContrasenya()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RecuperarContrasenya(RecuperarContrasenyaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = _usuarioDAL.GetUsuarioByEmail(model.Email);

            if (usuario != null)
            {
                try
                {
                    Console.WriteLine($"✅ Enviando correo a: {model.Email}");

                    // 1. Generar token único
                    string token = Guid.NewGuid().ToString();
                    DateTime expiracion = DateTime.UtcNow.AddHours(1);

                    // 2. Guardar token y expiración
                    _usuarioDAL.GuardarTokenRecuperacion(usuario.IdUsuario, token, expiracion);
                    Console.WriteLine($"🔐 Token generado: {token}");

                    // 3. Crear URL de restablecimiento
                    string link = Url.Action("RestablecerContrasenya", "Login", new { token }, Request.Scheme);
                    Console.WriteLine($"🔗 Link generado: {link}");

                    // 4. Enviar email
                    string html = $@"
                <p>Hola {usuario.Username},</p>
                <p>Haz clic en el siguiente enlace para restablecer tu contraseña:</p>
                <p><a href='{link}'>{link}</a></p>
                <p>Este enlace expirará en 1 hora.</p>";

                    EmailService.Enviar(model.Email, "Recuperación de contraseña - MySocialPet", html);

                    Console.WriteLine("📤 Correo enviado con éxito.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Error al enviar el correo: " + ex.Message);
                    // Si lo deseas, puedes mostrar un error genérico al usuario aquí
                }
            }

            TempData["Message"] = "Si el correo está registrado, recibirás un enlace para recuperar tu cuenta.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult RestablecerContrasenya(string token)
        {
            Console.WriteLine("[GET] RestablecerContrasenya - Token recibido: " + token);

            var usuario = _usuarioDAL.GetUsuarioByToken(token);
            if (usuario == null)
            {
                Console.WriteLine("[GET] Token inválido o expirado.");
                TempData["Message"] = "El enlace ha expirado o es inválido.";
                return RedirectToAction("Index");
            }

            Console.WriteLine("[GET] Token válido, mostrando formulario.");
            return View(new RestablecerContrasenyaViewModel { Token = token });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RestablecerContrasenya(RestablecerContrasenyaViewModel model)
        {
            Console.WriteLine("[POST] RestablecerContrasenya - Token: " + model.Token);

            if (!ModelState.IsValid)
            {
                Console.WriteLine("[POST] ModelState inválido.");
                return View(model);
            }

            var usuario = _usuarioDAL.GetUsuarioByToken(model.Token);
            if (usuario == null)
            {
                Console.WriteLine("[POST] Token inválido o expirado.");
                TempData["Message"] = "El enlace ha expirado o es inválido.";
                return RedirectToAction("Index");
            }

            Console.WriteLine("[POST] Token válido, actualizando contraseña...");
            _usuarioDAL.CambiarContrasenya(usuario, model.NuevaContrasenya);

            Console.WriteLine("[POST] Eliminando token de recuperación...");
            _usuarioDAL.EliminarTokenRecuperacion(usuario.IdUsuario);

            TempData["Message"] = "Contraseña actualizada correctamente.";
            Console.WriteLine("[POST] Contraseña actualizada, redirigiendo a Index.");
            return RedirectToAction("Index");
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
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}