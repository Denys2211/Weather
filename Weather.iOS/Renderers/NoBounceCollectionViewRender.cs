
using Weather.iOS.Renders;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CollectionView), typeof(NoBounceCollectionViewRender))]
namespace Weather.iOS.Renders
{
    public class NoBounceCollectionViewRender : CollectionViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<GroupableItemsView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
                Controller.CollectionView.Bounces = false;
        }
    }
}