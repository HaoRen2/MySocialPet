using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using MySocialPet.DAL;
using MySocialPet.Models.ViewModel.Perfil;
using MySocialPet.Tools;
using System.Security.Claims;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MySocialPet.Controllers
{
    [Authorize]
    public class PerfilController : Controller
    {
        private readonly UsuarioDAL _usuarioDal;
        private readonly IWebHostEnvironment _env;

        public PerfilController(UsuarioDAL usuarioDal, IWebHostEnvironment env)
        {
            _usuarioDal = usuarioDal;
            _env = env;
        }

        private int GetCurrentUserId()
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idStr, out int id) ? id : 0;
        }

        // GET: /Perfil
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int id = GetCurrentUserId();
            if (id <= 0) return RedirectToAction("Index", "Home");

            var data = await _usuarioDal.GetPerfilByIdAsync(id);
            if (data == null) return NotFound();

            // Avatares por defecto desde wwwroot/src/DefaultAvatar
            var folder = Path.Combine(_env.WebRootPath, "src", "DefaultAvatar");
            var patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.webp" };
            var files = new List<string>();

            if (Directory.Exists(folder))
            {
                foreach (var pattern in patterns)
                {
                    files.AddRange(Directory.GetFiles(folder, pattern)
                                            .Select(Path.GetFileName)
                                            .Where(fn => !string.IsNullOrEmpty(fn))!);
                }
            }

            data.DefaultAvatarFileNames = files
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(f => f)
                .ToList();

            return View(data);
        }

        // POST: /Perfil/SaveCuenta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCuenta(PerfilViewModel model)
        {
            int id = GetCurrentUserId();

            bool ok = await _usuarioDal.UpdatePerfilBasicoAsync(
                id,
                model.Username,
                model.Nombre,
                model.Apellido,
                model.Telefono,
                model.Direccion,
                model.Ciudad,
                model.Provincia,
                model.CodigoPostal
            );

            var msg = ok ? "Perfil actualizado." : "No se pudo actualizar el perfil.";

            if (Request.IsAjaxRequest())
                return Json(new { success = ok, message = msg });

            return RedirectWithMessage(msg);
        }

        // POST: /Perfil/SaveAvatar (subida directa o quitar)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAvatar(IFormFile? avatar, bool removeAvatar = false)
        {
            int id = GetCurrentUserId();
            bool ok = false;
            string msg;
            string? previewDataUrl = null;

            if (removeAvatar)
            {
                await _usuarioDal.RemoveAvatarAsync(id);
                msg = "Avatar eliminado.";
                ok = true;
                // usa uno por defecto visible sin recargar:
                previewDataUrl = Url.Content("~/src/DefaultAvatar/AvatarPerro.png");
            }
            else if (avatar != null && avatar.Length > 0)
            {
                var allowedContent = new[] { "image/png", "image/jpeg", "image/webp" };
                if (!allowedContent.Contains(avatar.ContentType))
                {
                    msg = "Formato no válido. Usa PNG/JPG/WEBP.";
                    return AjaxOrRedirect(false, msg);
                }
                if (avatar.Length > 30 * 1024 * 1024)
                {
                    msg = "La imagen no puede superar los 30 MB.";
                    return AjaxOrRedirect(false, msg);
                }

                using var ms = new MemoryStream();
                await avatar.CopyToAsync(ms);
                var bytes = ms.ToArray();

                await _usuarioDal.UpdateAvatarAsync(id, bytes);
                msg = "Avatar actualizado.";
                ok = true;

                // Previsualización con el MIME real recibido
                var mime = string.IsNullOrWhiteSpace(avatar.ContentType) ? "image/png" : avatar.ContentType;
                previewDataUrl = $"data:{mime};base64,{Convert.ToBase64String(bytes)}";
            }
            else
            {
                msg = "No se recibió ningún archivo.";
            }

            return AjaxOrRedirect(ok, msg, previewDataUrl);

            IActionResult AjaxOrRedirect(bool success, string message, string? preview = null)
            {
                if (Request.IsAjaxRequest())
                    return Json(new { success, message, previewDataUrl = preview });

                TempData["Message"] = message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Perfil/SetAvatarDefault (usar uno de wwwroot/src/DefaultAvatar/*)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetAvatarDefault(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return Json(new { success = false, message = "Archivo inválido." });

            var folder = Path.Combine(_env.WebRootPath, "src", "DefaultAvatar");
            var fullPath = Path.GetFullPath(Path.Combine(folder, fileName));
            var folderFull = Path.GetFullPath(folder);

            // Seguridad: solo dentro de la carpeta y que exista
            if (!fullPath.StartsWith(folderFull, StringComparison.OrdinalIgnoreCase) ||
                !System.IO.File.Exists(fullPath))
            {
                return Json(new { success = false, message = "El archivo no existe." });
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            int id = GetCurrentUserId();
            var ok = await _usuarioDal.UpdateAvatarAsync(id, bytes);

            // MIME según extensión
            var mime = MimeFromExtension(Path.GetExtension(fullPath));
            var previewDataUrl = $"data:{mime};base64,{Convert.ToBase64String(bytes)}";

            return Json(new { success = ok, message = ok ? "Avatar actualizado." : "No se pudo actualizar.", previewDataUrl });
        }

        // POST: /Perfil/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string CurrentPassword, string NewPassword, string ConfirmNewPassword)
        {
            if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword != ConfirmNewPassword)
            {
                return Finish(false, "Las contraseñas no coinciden.");
            }

            int id = GetCurrentUserId();
            var usuario = await _usuarioDal.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return Finish(false, "No se encontró el usuario.");
            }

            if (!PasswordHelper.VerifyPasswordHash(CurrentPassword, usuario.PasswordHash, usuario.PasswordSalt))
            {
                return Finish(false, "La contraseña actual no es correcta.");
            }

            _usuarioDal.CambiarContrasenya(usuario, NewPassword);
            return Finish(true, "Contraseña actualizada.");

            IActionResult Finish(bool success, string message)
            {
                if (Request.IsAjaxRequest())
                    return Json(new { success, message });

                TempData["Message"] = message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: /Perfil/ChangeEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(string NewEmail, string ConfirmNewEmail, string Password)
        {
            if (string.IsNullOrWhiteSpace(NewEmail) || NewEmail != ConfirmNewEmail)
            {
                return Finish(false, "Los correos no coinciden.");
            }

            int id = GetCurrentUserId();
            var usuario = await _usuarioDal.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return Finish(false, "No se encontró el usuario.");
            }

            if (!PasswordHelper.VerifyPasswordHash(Password, usuario.PasswordHash, usuario.PasswordSalt))
            {
                return Finish(false, "La contraseña no es válida.");
            }

            if (await _usuarioDal.EmailExistsExceptAsync(id, NewEmail))
            {
                return Finish(false, "Ese correo ya está en uso.");
            }

            var ok = await _usuarioDal.ChangeEmailAsync(id, NewEmail);
            return Finish(ok, ok ? "Correo actualizado." : "No se pudo actualizar el correo.");

            IActionResult Finish(bool success, string message)
            {
                if (Request.IsAjaxRequest())
                    return Json(new { success, message });

                TempData["Message"] = message;
                return RedirectToAction(nameof(Index));
            }
        }

        // Helpers
        private IActionResult RedirectWithMessage(string msg)
        {
            TempData["Message"] = msg;
            return RedirectToAction(nameof(Index));
        }

        private static string MimeFromExtension(string? ext)
        {
            ext = (ext ?? "").ToLowerInvariant();
            return ext switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".webp" => "image/webp",
                _ => "image/png"
            };
        }
    }

    // Helper para detectar AJAX
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest",
                                 StringComparison.OrdinalIgnoreCase);
        }
    }
}
