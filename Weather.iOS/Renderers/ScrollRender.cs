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

            Scrolled -= ScrollviewRender_Scrolled;
            Scrolled += ScrollviewRender_Scrolled;

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                this.ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
            }
        }

        private void ScrollviewRender_Scrolled(object sender, System.EventArgs e)
        {
            var scroll = Element as CustomScrollView;

            if (scroll != null)
            {
                if (scroll.ScrollY <= 0)
                {
                    Bounces = true;
                }
                else
                {
                    Bounces = false;
                }
            }
            
        }
    }
}