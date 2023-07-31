using Weather.Extensions;
using Weather.Operations;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Weather.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomImage : ContentView
	{
        private const string PathToImages = "Weather.Images";
        private ShowBaseOperation entityGraphicOperation;

        public CustomImage ()
		{
			InitializeComponent ();
		}

        public static readonly BindableProperty EntityImageSourceProperty = BindableProperty.Create(
            nameof(EntityImageSource), typeof(string), typeof(CustomImage),
            default(string), propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (newValue is null)
                {
                    return;
                }

                var obj = bindable as CustomImage;

                var value = newValue.ToString();

                obj.SetEntityIcon($"{PathToImages}.{value}.png");
            });

        public string EntityImageSource
        {
            get => GetValue(EntityImageSourceProperty) as string;
            set => SetValue(EntityImageSourceProperty, value);
        }

        private bool SetEntityIcon(string resourceId)
        {
            entityGraphicOperation?.Dispose();

            var entityIcon = BitmapExtensions.LoadBitmapResource(typeof(CustomImage), resourceId);

            if (entityIcon == null)
            {
                return false;
            }

            entityGraphicOperation = new ShowBaseOperation(entityIcon);
            EntityImageView.Content = entityGraphicOperation.CanvasView;

            return true;
        }
    }
}