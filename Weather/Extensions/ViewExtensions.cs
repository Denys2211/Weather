using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Weather.Extensions
{
    public static class ViewExtensions
    {
        public static Task<bool> ColorTo(this VisualElement self, Color fromColor, Color toColor, Action<Color> callback, uint length = 250, Easing easing = null)
        {
            Func<double, Color> transform = (t) =>
              Color.FromRgba(fromColor.R + t * (toColor.R - fromColor.R),
                             fromColor.G + t * (toColor.G - fromColor.G),
                             fromColor.B + t * (toColor.B - fromColor.B),
                             fromColor.A + t * (toColor.A - fromColor.A));
            return ColorAnimation(self, "ColorTo", transform, callback, length, easing);
        }

        public static void CancelAnimation(this VisualElement self)
        {
            self.AbortAnimation("ColorTo");
        }

        static Task<bool> ColorAnimation(VisualElement element, string name, Func<double, Color> transform, Action<Color> callback, uint length, Easing easing)
        {
            easing = easing ?? Easing.Linear;
            var taskCompletionSource = new TaskCompletionSource<bool>();

            element.Animate<Color>(name, transform, callback, 16, length, easing, (v, c) => taskCompletionSource.SetResult(c));
            return taskCompletionSource.Task;
        }

        public static Task<bool> AnimateTo(this VisualElement element, int moveTo, Action<double> callback, uint length = 250, Easing easing = null)
        {
            easing = easing ?? Easing.Linear;
            var taskCompletionSource = new TaskCompletionSource<bool>();

            Func<double, double> transform = (t) => t + moveTo;

            element.Animate<double>("AnimatedTo", transform, callback, 16, length, easing, (v, c) => taskCompletionSource.SetResult(c));
            return taskCompletionSource.Task;
        }
    }
}
