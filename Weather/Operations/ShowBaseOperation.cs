using SkiaSharp;
using SkiaSharp.Views.Forms;
using Weather.Extensions;

namespace Weather.Operations
{
    public class ShowBaseOperation : OperationBase
    {
        private SKBitmap _device;

        public ShowBaseOperation(SKBitmap device)
        {
            _device = device;
            CanvasView = new SKCanvasView();
            CanvasView.PaintSurface += OnCanvasViewPaintSurface;
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            using (SKPaint paint = new SKPaint())
            {
                //if (_isDeviceImage)
                //{
                //    canvas.DrawBitmap(_device, new SKRectI(info.Rect.Left, info.Rect.Top, info.Rect.Right - info.Rect.Right / 7, info.Rect.Bottom - info.Rect.Bottom / 7), BitmapStretch.Uniform, paint: paint);
                //}
                //else
                {
                    canvas.DrawBitmap(_device, info.Rect, BitmapStretch.Uniform, paint: paint);
                }
            }
        }

        public override void Dispose()
        {
            _device = null;
            CanvasView.PaintSurface -= OnCanvasViewPaintSurface;
        }
    }
}
