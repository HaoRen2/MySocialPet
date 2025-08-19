using System;
using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Perfil
{
    public class PerfilViewModel
    {
        // Identidad / cuenta
        public int IdUsuario { get; set; }
        public string Username { get; set; } = "";
        [EmailAddress]
        public string Email { get; set; } = "";
        public byte[]? AvatarFoto { get; set; }
        public DateTime FechaRegistro { get; set; }

        public string? TipoUsuarioNombre { get; set; }

        // Datos personales (opcionales, editables en "Cuenta")
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        [Phone]
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }

        [Display(Name = "Provincia")]
        public string? Provincia { get; set; }

        [Display(Name = "Código Postal")]
        public string? CodigoPostal { get; set; }

        // Actividad (solo lectura)
        public int MascotasCount { get; set; }
        public int AlbumesCount { get; set; }
        public int DiscusionesCount { get; set; }
        public int MensajesCount { get; set; }

        public List<string> DefaultAvatarFileNames { get; set; } = new();
    }
}
