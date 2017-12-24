using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using SpotFinder.Droid.Renderers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(GenericEntryRenderer))]
namespace SpotFinder.Droid.Renderers
{
    public class GenericEntryRenderer : EntryRenderer
    {
        public GenericEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control == null || Element == null || e.OldElement != null) return;

            
            Control.Background = Resources.GetDrawable(Resource.Drawable.noBorderEditText);
            Control.SetPadding(45, 30, 45, 30);
        }
    }
}