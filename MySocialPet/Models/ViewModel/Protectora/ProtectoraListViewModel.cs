using MySocialPet.Models.Autenticacion;
using MySocialPet.Models.Mascotas;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Protectora
{
    // Listado de todas las protector@s (Index)
    public class ProtectoraListViewModel
    {
        public List<ProtectoraViewModel> Protectoras { get; set; } = new();
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }


    // Una protectora en particular (para cards o detalle)
    public class ProtectoraViewModel
    {
        // === Campos de Usuario ====
        [EmailAddress]
        public string Email { get; set; } = "";
        public byte[]? AvatarFoto { get; set; }
        [Phone]
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }

        [Display(Name = "Provincia")]
        public string? Provincia { get; set; }

        [Display(Name = "Código Postal")]
        public string? CodigoPostal { get; set; }


        // === Campos que ya existen en BD ===
        public int IdUsuario { get; set; }
        public int IdProtectora { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Web { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Introduccion { get; set; } = string.Empty;

        // === Derivados / calculados ===
        public int NumPerros { get; set; }
        public int NumGatos { get; set; }
        public int NumMascotas => NumPerros + NumGatos; // calculado automáticamente
    }

    public class ProtectoraFilter
    {
        public string? Q { get; set; }
        public string? Ciudad { get; set; }
        public bool? SoloActivas { get; set; }
        public bool? SoloConWeb { get; set; }
        public int? IdEspecie { get; set; }     // <<— necesario para el filtro normalizado
        public string Orden { get; set; } = "recientes";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }


    public class ProtectoraFilterDataVm
    {
        public List<string> Ciudades { get; set; } = new();
        public int TotalActivas { get; set; }
        public int TotalConWeb { get; set; }
        public int Total { get; set; }

        // Facetas opcionales
        public int TotalConPerros { get; set; }
        public int TotalConGatos { get; set; }
    }


}
