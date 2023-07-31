using SkiaSharp.Views.Forms;

namespace Weather.Operations
{
    public class OperationBase : IOperationBase
    {
        public SKCanvasView CanvasView { get; set; }

        public virtual void Dispose()
        {
        }
    }
}
