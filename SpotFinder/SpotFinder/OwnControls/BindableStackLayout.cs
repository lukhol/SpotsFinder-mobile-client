using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpotFinder.OwnControls
{
    public class BindableStackLayout : StackLayout
    {
        public IList<View> ChildrenList { get; set; }

        public static readonly BindableProperty ChildrenListProperty
            = BindableProperty.Create(
                nameof(ChildrenList),
                typeof(IList<View>),
                typeof(BindableStackLayout),
                new List<View>(),
                propertyChanged: (b, o, n) => 
                {
                    var thisStackLayout = (BindableStackLayout)b;
                    var list = (IList<View>)n;

                    foreach (var item in list)
                        thisStackLayout.Children.Add(item);
                });
    }
}
