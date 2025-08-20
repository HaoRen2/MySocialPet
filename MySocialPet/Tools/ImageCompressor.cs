using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace MySocialPet.Tools
{
    public static class ImageCompressor
    {
        public class Result
        {
            public byte[] Data { get; set; }
            public string ContentType { get; set; }  // "image/jpeg" o "image/png"
            public int Width { get; set; }
            public int Height { get; set; }
            public int? QualityUsed { get; set; }    // solo para JPEG
            public long Bytes => Data != null ? Data.LongLength : 0;
        }

        /// <summary>
        /// Comprime/redimensiona un archivo subido hasta quedar por debajo de maxBytes.
        /// - Si keepTransparency == false (por defecto), fuerza JPEG (ideal para fotos).
        /// - Si keepTransparency == true, mantiene transparencia con PNG (sin pérdidas).
        /// </summary>
        public static async Task<Result> CompressToUnderAsync(
            IFormFile file,
            int maxBytes = 2 * 1024 * 1024,
            int? maxWidth = 1920,
            bool keepTransparency = false,
            int initialQuality = 85,
            int minQuality = 40,
            int qualityStep = 5)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Archivo inválido.", nameof(file));

            using (var stream = file.OpenReadStream())
                return await CompressToUnderAsync(stream, maxBytes, maxWidth, keepTransparency, initialQuality, minQuality, qualityStep);
        }

        public static async Task<Result> CompressToUnderAsync(
            byte[] bytes,
            int maxBytes = 2 * 1024 * 1024,
            int? maxWidth = 1920,
            bool keepTransparency = false,
            int initialQuality = 85,
            int minQuality = 40,
            int qualityStep = 5)
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentException("Bytes inválidos.", nameof(bytes));

            using (var ms = new MemoryStream(bytes))
                return await CompressToUnderAsync(ms, maxBytes, maxWidth, keepTransparency, initialQuality, minQuality, qualityStep);
        }

        public static async Task<Result> CompressToUnderAsync(
            Stream input,
            int maxBytes = 2 * 1024 * 1024,
            int? maxWidth = 1920,
            bool keepTransparency = false,
            int initialQuality = 85,
            int minQuality = 40,
            int qualityStep = 5)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (maxBytes <= 0) throw new ArgumentOutOfRangeException(nameof(maxBytes));
            if (initialQuality < 1 || initialQuality > 100) throw new ArgumentOutOfRangeException(nameof(initialQuality));
            if (minQuality < 1 || minQuality >= initialQuality) throw new ArgumentOutOfRangeException(nameof(minQuality));
            if (qualityStep <= 0) throw new ArgumentOutOfRangeException(nameof(qualityStep));

            input.Position = 0;

            using (var image = await Image.LoadAsync(input))
            {
                // 1) Auto-orientar por EXIF
                image.Mutate(x => x.AutoOrient());

                // 2) Redimensionar si el ancho excede maxWidth
                if (maxWidth.HasValue && image.Width > maxWidth.Value)
                {
                    var target = new Size(maxWidth.Value, 0); // mantiene aspecto
                    image.Mutate(x => x.Resize(new ResizeOptions { Mode = ResizeMode.Max, Size = target }));
                }

                // Si NO queremos transparencia => JPEG con calidad progresiva.
                if (!keepTransparency)
                    return await EncodeJpegUnderAsync(image, maxBytes, initialQuality, minQuality, qualityStep);

                // Si queremos transparencia => PNG (sin pérdidas).
                return await EncodePngUnderAsync(image, maxBytes);
            }
        }

        // ===== Internos =====

        private static async Task<Result> EncodeJpegUnderAsync(Image image, int maxBytes, int initialQuality, int minQuality, int step)
        {
            int currentQuality = initialQuality;
            int currentWidth = image.Width;
            int attempts = 0;

            while (true)
            {
                byte[] candidate = await EncodeJpegAsync(image, currentQuality);
                if (candidate.LongLength <= maxBytes)
                {
                    return new Result
                    {
                        Data = candidate,
                        ContentType = "image/jpeg",
                        Width = image.Width,
                        Height = image.Height,
                        QualityUsed = currentQuality
                    };
                }

                // Bajar calidad
                if (currentQuality - step >= minQuality)
                {
                    currentQuality -= step;
                    continue;
                }

                // Calidad al mínimo: reducir resolución un 10% y reintentar
                int nextWidth = (int)Math.Round(currentWidth * 0.9);
                if (nextWidth < 640 && image.Width <= 640)
                {
                    // No podemos bajar más; devolvemos el mejor intento (minQuality)
                    byte[] lastTry = await EncodeJpegAsync(image, currentQuality);
                    return new Result
                    {
                        Data = lastTry,
                        ContentType = "image/jpeg",
                        Width = image.Width,
                        Height = image.Height,
                        QualityUsed = currentQuality
                    };
                }

                currentWidth = Math.Max(nextWidth, 640);
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(currentWidth, 0)
                }));
                currentQuality = initialQuality;

                attempts++;
                if (attempts > 10) // tope seguridad
                {
                    byte[] lastTry = await EncodeJpegAsync(image, currentQuality);
                    return new Result
                    {
                        Data = lastTry,
                        ContentType = "image/jpeg",
                        Width = image.Width,
                        Height = image.Height,
                        QualityUsed = currentQuality
                    };
                }
            }
        }

        private static async Task<Result> EncodePngUnderAsync(Image image, int maxBytes)
        {
            // PNG no tiene "calidad": probamos compresión alta y, si no basta, reducimos resolución.
            int currentWidth = image.Width;
            int attempts = 0;

            while (true)
            {
                byte[] candidate = await EncodePngAsync(image, compressionLevel: PngCompressionLevel.Level9);
                if (candidate.LongLength <= maxBytes)
                {
                    return new Result
                    {
                        Data = candidate,
                        ContentType = "image/png",
                        Width = image.Width,
                        Height = image.Height,
                        QualityUsed = null
                    };
                }

                int nextWidth = (int)Math.Round(currentWidth * 0.9);
                if (nextWidth < 640 && image.Width <= 640)
                {
                    // No podemos bajar más; devolvemos el mejor intento
                    byte[] lastTry = await EncodePngAsync(image, PngCompressionLevel.Level9);
                    return new Result
                    {
                        Data = lastTry,
                        ContentType = "image/png",
                        Width = image.Width,
                        Height = image.Height,
                        QualityUsed = null
                    };
                }

                currentWidth = Math.Max(nextWidth, 640);
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(currentWidth, 0)
                }));

                attempts++;
                if (attempts > 10)
                {
                    byte[] lastTry = await EncodePngAsync(image, PngCompressionLevel.Level9);
                    return new Result
                    {
                        Data = lastTry,
                        ContentType = "image/png",
                        Width = image.Width,
                        Height = image.Height,
                        QualityUsed = null
                    };
                }
            }
        }

        private static async Task<byte[]> EncodeJpegAsync(Image image, int quality)
        {
            using (var ms = new MemoryStream())
            {
                var enc = new JpegEncoder { Quality = quality };
                await image.SaveAsJpegAsync(ms, enc);
                return ms.ToArray();
            }
        }

        private static async Task<byte[]> EncodePngAsync(Image image, PngCompressionLevel compressionLevel)
        {
            using (var ms = new MemoryStream())
            {
                var enc = new PngEncoder { CompressionLevel = compressionLevel };
                await image.SaveAsPngAsync(ms, enc);
                return ms.ToArray();
            }
        }
    }
}
