using System.ComponentModel.DataAnnotations;

namespace MySocialPet.Models.ViewModel.Foros
{
    public class EnviarMensajeViewModel
    {
        [Required]
        public int IdForo { get; set; }

        [Required]
        public int IdDiscusion { get; set; }

        public int? IdMensajePadre { get; set; } // null si es nuevo mensaje

        [Required(ErrorMessage = "El contenido del mensaje es obligatorio.")]
        [StringLength(2000, ErrorMessage = "El mensaje no puede superar los 2000 caracteres.")]
        public string Contenido { get; set; }

        [AllowedExtensions(new string[] { ".png", ".jpg", ".jpeg", ".gif" }, ErrorMessage = "Solo se permiten imágenes PNG, JPG o GIF.")]
        public IFormFile? Imagen { get; set; }
        public string Slug { get; set; } = string.Empty;

    }

    // Validación de tamaño máximo
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null && file.Length > _maxFileSize)
            {
                return new ValidationResult(ErrorMessage ?? $"El archivo no puede superar los {_maxFileSize / 1024 / 1024} MB.");
            }
            return ValidationResult.Success;
        }
    }

    // Validación de extensiones permitidas
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(ErrorMessage ?? $"Extensión no permitida. Solo: {string.Join(", ", _extensions)}");
                }
            }
            return ValidationResult.Success;
        }
    }
}

