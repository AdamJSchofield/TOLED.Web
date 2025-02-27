using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TOLED.Web.Data.Models;

namespace TOLED.Web.Encoders
{
    public static class Ssd1309Encoder
    {
        public async static Task<Ssd1309EncodingResult> EncodeImageAsync(byte[] rawImage, PPImageMutateOptions options, bool isWebPreview = false)
        {
            using var image = Image.Load<L8>(rawImage);

            using var displayDataStream = new MemoryStream();

            if (isWebPreview)
            {
                image.Mutate(i => i.MutatePPImage(options, isWebPreview));

                if (image.Frames.Count > 1)
                {
                    await image.SaveAsGifAsync(displayDataStream);
                }
                else
                {
                    await image.SaveAsBmpAsync(displayDataStream, new BmpEncoder
                    {
                        BitsPerPixel = BmpBitsPerPixel.Pixel1,
                        SkipMetadata = true,
                        SupportTransparency = false,
                    });
                }

                return new Ssd1309EncodingResult
                {
                    DisplayData = displayDataStream.ToArray(),
                    DisplayFrames = image.Frames.Count
                };
            }


            for (int i = 0; i < image.Frames.Count; i++)
            {
                using var frame = image.Frames.CloneFrame(i);
                frame.Mutate(i => i.MutatePPImage(options, isWebPreview));
                using var encodedFrame = new MemoryStream();
                frame.SaveAsBmp(encodedFrame, new BmpEncoder
                {
                    BitsPerPixel = BmpBitsPerPixel.Pixel1,
                    SkipMetadata = true,
                });
                await displayDataStream.WriteAsync(encodedFrame.ToArray().TakeLast(1024).ToArray());
            }

            return new Ssd1309EncodingResult
            {
                DisplayData = displayDataStream.ToArray(),
                DisplayFrames = image.Frames.Count,
            };
        }

        public static async Task<string> GetDevicePreview(byte[] rawImage, PPImageMutateOptions options)
        {
            var encodingResult = await EncodeImageAsync(rawImage, options, true);
            using var image = Image.Load<L8>(encodingResult.DisplayData);
            return image.ToBase64String(image.Frames.Count > 1 ? GifFormat.Instance : BmpFormat.Instance);
        }

        static void MutatePPImage(this IImageProcessingContext context, PPImageMutateOptions options, bool isWebPreview)
        {
            context.RotateFlip(options.RotateMode, options.FlipMode);

            context.Resize(new ResizeOptions()
            {
                Size = new Size(128, 64),
                Mode = options.Mode,
                Position = options.Position,
                PadColor = Color.Parse(options.PadColor)
            });


            // Get dithering algo
            var foundAlgo = PPImageMutateOptions.DitheringAlgorithms.TryGetValue(options.DitheringAlglorithm, out var algo);
            context.BinaryDither(foundAlgo && algo != null ? algo : KnownDitherings.FloydSteinberg, Color.White, Color.Black);
            if (options.InvertColors)
            {
                context.Invert();
            }
        }
    }

    public class Ssd1309EncodingResult
    {
        public byte[] DisplayData { get; set; } = default!;
        public int DisplayFrames { get; set; }
    }
}
