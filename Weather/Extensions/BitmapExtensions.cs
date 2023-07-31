using SkiaSharp;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace Weather.Extensions
{
    static class BitmapExtensions
    {
        public static SKBitmap LoadBitmapResource(Type type, string resourceID)
        {
            Assembly assembly = type.GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(resourceID);

            if (stream == null)
                return null;

            return SKBitmap.Decode(stream);
        }

        public static SKBitmap LoadBitmapResource(string resourceID)
        {
            if (Application.Current.Resources.TryGetValue(resourceID, out object value))
            {
                return value as SKBitmap;
            }

            return null;
        }

        public static SKBitmap LoadBitmapResource(Stream photoContentStream)
        {
            using var stream = new SKManagedStream(photoContentStream, false);
            SKImage img = SKImage.FromEncodedData(stream);
            SKBitmap decodedBitmap = SKBitmap.FromImage(img);
            return decodedBitmap;
        }

        public static uint RgbaMakePixel(byte red, byte green, byte blue, byte alpha = 255)
        {
            return (uint)(alpha << 24 | blue << 16 | green << 8 | red);
        }

        public static void RgbaGetBytes(this uint pixel, out byte red, out byte green, out byte blue, out byte alpha)
        {
            red = (byte)pixel;
            green = (byte)(pixel >> 8);
            blue = (byte)(pixel >> 16);
            alpha = (byte)(pixel >> 24);
        }

        public static uint BgraMakePixel(byte blue, byte green, byte red, byte alpha = 255)
        {
            return (uint)(alpha << 24 | red << 16 | green << 8 | blue);
        }

        public static void BgraGetBytes(this uint pixel, out byte blue, out byte green, out byte red, out byte alpha)
        {
            blue = (byte)pixel;
            green = (byte)(pixel >> 8);
            red = (byte)(pixel >> 16);
            alpha = (byte)(pixel >> 24);
        }

        public static void DrawBitmap(this SKCanvas canvas, SKBitmap bitmap, SKRect dest,
                                      BitmapStretch stretch,
                                      BitmapAlignment horizontal = BitmapAlignment.Center,
                                      BitmapAlignment vertical = BitmapAlignment.Center,
                                      SKPaint paint = null)
        {
            try
            {
                if (stretch == BitmapStretch.Fill)
                {
                    canvas.DrawBitmap(bitmap, dest, paint);
                }
                else
                {
                    float scale = 1;

                    switch (stretch)
                    {
                        case BitmapStretch.None:
                            break;

                        case BitmapStretch.Uniform:
                            scale = Math.Min(dest.Width / bitmap.Width, dest.Height / bitmap.Height);
                            break;

                        case BitmapStretch.UniformToFill:
                            scale = Math.Max(dest.Width / bitmap.Width, dest.Height / bitmap.Height);
                            break;
                    }

                    SKRect display = CalculateDisplayRect(dest, scale * bitmap.Width, scale * bitmap.Height,
                                                          horizontal, vertical);

                    canvas.DrawBitmap(bitmap, display, paint);
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        public static void DrawBitmap(this SKCanvas canvas, SKBitmap bitmap, SKRect source, SKRect dest,
                                      BitmapStretch stretch,
                                      BitmapAlignment horizontal = BitmapAlignment.Center,
                                      BitmapAlignment vertical = BitmapAlignment.Center,
                                      SKPaint paint = null)
        {
            try
            {
                if (stretch == BitmapStretch.Fill)
                {
                    canvas.DrawBitmap(bitmap, source, dest, paint);
                }
                else
                {
                    float scale = 1;

                    switch (stretch)
                    {
                        case BitmapStretch.None:
                            break;

                        case BitmapStretch.Uniform:
                            scale = Math.Min(dest.Width / source.Width, dest.Height / source.Height);
                            break;

                        case BitmapStretch.UniformToFill:
                            scale = Math.Max(dest.Width / source.Width, dest.Height / source.Height);
                            break;
                    }

                    SKRect display = CalculateDisplayRect(dest, scale * source.Width, scale * source.Height,
                                                          horizontal, vertical);

                    canvas.DrawBitmap(bitmap, source, display, paint);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        static SKRect CalculateDisplayRect(SKRect dest, float bmpWidth, float bmpHeight,
                                           BitmapAlignment horizontal, BitmapAlignment vertical)
        {
            float x = 0;
            float y = 0;

            switch (horizontal)
            {
                case BitmapAlignment.Center:
                    x = (dest.Width - bmpWidth) / 2;
                    break;

                case BitmapAlignment.Start:
                    break;

                case BitmapAlignment.End:
                    x = dest.Width - bmpWidth;
                    break;
            }

            switch (vertical)
            {
                case BitmapAlignment.Center:
                    y = (dest.Height - bmpHeight) / 2;
                    break;

                case BitmapAlignment.Start:
                    break;

                case BitmapAlignment.End:
                    y = dest.Height - bmpHeight;
                    break;
            }

            x += dest.Left;
            y += dest.Top;

            return new SKRect(x, y, x + bmpWidth, y + bmpHeight);
        }
    }

    public enum BitmapStretch
    {
        None,
        Fill,
        Uniform,
        UniformToFill,
        AspectFit = Uniform,
        AspectFill = UniformToFill
    }

    public enum BitmapAlignment
    {
        Start,
        Center,
        End
    }
}
