using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Weather.iOS.Renderers;
using Weather.CustomControls;

[assembly: ExportRenderer(typeof(CustomScrollView), typeof(ScrollviewRender))]
namespace Weather.iOS.Renderers
{
    class ScrollviewRender : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                this.ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
            }
        }
    }
}