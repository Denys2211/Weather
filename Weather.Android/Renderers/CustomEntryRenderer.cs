using System;
using Android.Graphics.Drawables;
using Weather.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Weather.CustomControls.CustomEntry), typeof(CustomEntryRenderer))]
namespace Weather.Droid.Renderers
{
    [Obsolete]
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(Android.Graphics.Color.Transparent);
                this.Control.SetBackgroundDrawable(gd);
            }
        }
    }
}

