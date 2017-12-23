using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace SpotFinder.OwnControls
{
    public class BindableStackLayout : StackLayout
    {
        public ObservableCollection<View> ChildrenList { get; set; }

        public static readonly BindableProperty ChildrenListProperty
            = BindableProperty.Create(
                nameof(ChildrenList),
                typeof(ObservableCollection<View>),
                typeof(BindableStackLayout),
                new ObservableCollection<View>(),
                propertyChanged: (b, o, n) => 
                {
                    var thisStackLayout = (BindableStackLayout)b;
                    var list = (ObservableCollection<View>)n;

                    if (list == null)
                        return;

                    foreach (var item in list)
                        thisStackLayout.Children.Add(item);
                });
    }
}
