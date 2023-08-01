using Xamarin.Forms;

namespace Weather.CustomControls
{
    public class CustomScrollView : ScrollView
    {
        public bool IsJamper { get; set; }    

        protected override void ChangeVisualState()
        {
            base.ChangeVisualState();
        }
    }
}
