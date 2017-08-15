using SpotFinder.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Picker), typeof(GenericPickerRenderer))]
namespace SpotFinder.Droid.Renderers
{
    public class GenericPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (Control == null || Element == null || e.OldElement != null) return;


            Control.Background = Resources.GetDrawable(Resource.Drawable.noBorderEditText);
            Control.SetPadding(45, 30, 45, 30);
        }
    }
}