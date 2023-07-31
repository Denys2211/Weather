using SkiaSharp;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;

namespace Weather.Operations
{
    public static class OperationsImage
    {
        public static Stream DrawSVG(Stream stream)
        {
            var svg = new SkiaSharp.Extended.Svg.SKSvg();
            svg.Load(stream);
            using (var bitMap = new SKBitmap((int)svg.CanvasSize.Width, (int)svg.CanvasSize.Height))
            using (SKCanvas canvas = new SKCanvas(bitMap))
            {
                canvas.DrawPicture(svg.Picture);
                canvas.Flush();
                canvas.Save();

                using (SKImage image = SKImage.FromBitmap(bitMap))
                {
                    using (SKData data = image.Encode())
                    {
                        MemoryStream memStream = new MemoryStream();
                        data.SaveTo(memStream);
                        memStream.Seek(0, SeekOrigin.Begin);
                        return memStream;
                    }
                }
            }
        }

        public static Stream DrawPNG(Stream stream)
        {
            var sKBitmap = SKBitmap.Decode(stream);
            SKImage image = SKImage.FromBitmap(sKBitmap);
            //SKImage image = SKImage.FromPixels(sKBitmap.PeekPixels());

            using (SKData data = image.Encode())
            {
                MemoryStream memStream = new MemoryStream();
                data.SaveTo(memStream);
                memStream.Seek(0, SeekOrigin.Begin);
                return memStream;
            }
        }

        public static SKBitmap DrawBitmapPNG(Stream stream)
        {
            return SKBitmap.Decode(stream);
        }

        public static SKBitmap DrawBitmapSVG(Stream stream)
        {
            var svg = new SkiaSharp.Extended.Svg.SKSvg(new SKSize(100, 100));
            svg.Load(stream);
            using (var bitMap = new SKBitmap((int)svg.CanvasSize.Width, (int)svg.CanvasSize.Height))
            using (SKCanvas canvas = new SKCanvas(bitMap))
            {
                canvas.DrawPicture(svg.Picture);
                canvas.Flush();
                canvas.Save();

                using (SKImage image = SKImage.FromBitmap(bitMap))
                {
                    using (SKData data = image.Encode())
                    {
                        MemoryStream memStream = new MemoryStream();
                        data.SaveTo(memStream);
                        memStream.Seek(0, SeekOrigin.Begin);
                        return SKBitmap.Decode(memStream);
                    }
                }
            }
        }

        public static async Task<byte[]> ConvertImageSourceToBytesAsync(ImageSource imageSource)
        {
            Stream stream = await ((StreamImageSource)imageSource).Stream(CancellationToken.None);
            byte[] bytesAvailable = new byte[stream.Length];
            stream.Read(bytesAvailable, 0, bytesAvailable.Length);

            return bytesAvailable;
        }
    }
}
