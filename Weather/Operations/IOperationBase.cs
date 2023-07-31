using SkiaSharp.Views.Forms;
using System;

namespace Weather.Operations
{
    interface IOperationBase : IDisposable
    {
        SKCanvasView CanvasView { get; set; }
    }
}
