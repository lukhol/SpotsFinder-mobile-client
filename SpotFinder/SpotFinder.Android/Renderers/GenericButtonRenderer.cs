using Android.Content;
using SpotFinder.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(GenericButtonRenderer))]
namespace SpotFinder.Droid.Renderers
{
    public class GenericButtonRenderer : ButtonRenderer
    {
        public GenericButtonRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            var button = this.Control;
            button.SetAllCaps(false);
        }
    }
}