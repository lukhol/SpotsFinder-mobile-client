using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SpotFinder.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            Xamarin.FormsMaps.Init("4A7AvQIrWtWAg4md0gaO~Ofe5q5yQS-mopM0hECCOwQ~AiFV5OLW34oCaksRSfPTcwpUEXK1HH2IE4IhAZx7mi0aIMQ45gMxqCvjq9TMTGGK");
            this.InitializeComponent();
            LoadApplication(new SpotFinder.App());
        }
    }
}
