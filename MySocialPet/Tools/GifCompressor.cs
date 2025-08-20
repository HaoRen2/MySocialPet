using ImageMagick;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MySocialPet.Tools
{
    public static class GifCompressor //compress GIFs to under a certain size
    {
        public class Result
        {
            public byte[] Data { get; set; }
            public string ContentType { get; set; } = "image/gif";
            public int Width { get; set; }
            public int Height { get; set; }
            public long Bytes => Data?.LongLength ?? 0;
            public int Frames { get; set; }
        }

        public static async Task<Result> CompressToUnderAsync(
            IFormFile file,
            int maxBytes = 2 * 1024 * 1024,
            int? maxWidth = 640
        )
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Archivo inválido.", nameof(file));

            await using var stream = file.OpenReadStream();
            return await CompressToUnderAsync(stream, maxBytes, maxWidth);
        }

        public static async Task<Result> CompressToUnderAsync(
            byte[] bytes,
            int maxBytes = 2 * 1024 * 1024,
            int? maxWidth = 640
        )
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentException("Bytes inválidos.", nameof(bytes));

            await using var ms = new MemoryStream(bytes);
            return await CompressToUnderAsync(ms, maxBytes, maxWidth);
        }

        public static async Task<Result> CompressToUnderAsync(
            Stream input,
            int maxBytes = 2 * 1024 * 1024,
            int? maxWidth = 640
        )
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (maxBytes <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxBytes));

            using var frames = new MagickImageCollection();
            await frames.ReadAsync(input);

            if (frames.Count == 0)
                throw new ArgumentException("El GIF no contiene frames válidos.");

            frames.Coalesce(); // Normaliza offsets

            if (maxWidth.HasValue && frames[0].Width > (uint)maxWidth.Value)
                ScaleAllFrames(frames, maxWidth.Value);

            int currentColors = 256;
            int attempts = 0;

            while (true)
            {
                var candidateData = WriteToBytes(frames, currentColors);

                if (candidateData.Length <= maxBytes)
                    return new Result
                    {
                        Data = candidateData,
                        Width = (int)frames[0].Width,
                        Height = (int)frames[0].Height,
                        Frames = frames.Count
                    };

                // Reducir colores primero
                if (currentColors > 32)
                {
                    currentColors = Math.Max(32, currentColors / 2);
                }
                else
                {
                    int nextWidth = (int)Math.Round(frames[0].Width * 0.9);
                    if (nextWidth < 200)
                        return Finish(frames, currentColors);

                    ScaleAllFrames(frames, nextWidth);
                    currentColors = 256;
                }

                attempts++;
                if (attempts > 15)
                    return Finish(frames, currentColors);
            }
        }

        // --- Helpers ---
        private static void ScaleAllFrames(MagickImageCollection frames, int targetWidth)
        {
            foreach (var f in frames)
            {
                double scale = (double)targetWidth / f.Width;
                int newHeight = Math.Max(1, (int)Math.Round(f.Height * scale));

                f.Resize((uint)targetWidth, (uint)newHeight);
                f.Page = new MagickGeometry(0, 0, f.Width, f.Height); // limpia offsets
            }
        }

        private static byte[] WriteToBytes(MagickImageCollection frames, int colors)
        {
            var q = new QuantizeSettings
            {
                Colors = (uint)colors,
                DitherMethod = DitherMethod.FloydSteinberg,
            };

            using var optimizedFrames = new MagickImageCollection();
            foreach (var frame in frames)
                optimizedFrames.Add(frame.Clone());

            optimizedFrames.Quantize(q);
            optimizedFrames.OptimizePlus(); // suficiente para GIF animado

            using var ms = new MemoryStream();
            optimizedFrames.Write(ms, MagickFormat.Gif);
            return ms.ToArray();
        }

        private static Result Finish(MagickImageCollection frames, int colors)
        {
            var data = WriteToBytes(frames, colors);
            return new Result
            {
                Data = data,
                Width = (int)frames[0].Width,
                Height = (int)frames[0].Height,
                Frames = frames.Count
            };
        }
    }
}
