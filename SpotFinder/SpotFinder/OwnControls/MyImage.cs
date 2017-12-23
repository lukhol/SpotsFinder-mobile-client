using Xamarin.Forms;

namespace SpotFinder.OwnControls
{
    public class MyImage : Image
    {
        private string base64Representation;
        public string Base64Representation
        {
            get => base64Representation;
            set
            {
                base64Representation = value;
            }
        }

        public void RemoveFromParent()
        {
            var parent = (StackLayout)this.Parent;
            parent.Children.Remove(this);
        }
    }
}
