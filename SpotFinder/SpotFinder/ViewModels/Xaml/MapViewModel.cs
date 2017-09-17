using Microsoft.Practices.ServiceLocation;
using SpotFinder.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace SpotFinder.ViewModels.Xaml
{
    public class MapViewModel : BaseViewModel
    {
        public MapViewModel()
        {
            SearchCommand.Execute(null);
        }

        public Position DeviceLocation
        {
            get
            {
                var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();
                var myPosition = new Position(reportManager.Location.Latitude, reportManager.Location.Longitude);
                return myPosition;
            }
        }

        public ICommand SearchCommand => new Command(Test);

        private async void Test()
        {
            IsBusy = true;
            await Task.Delay(4000);
            IsBusy = false;
        }
    }
}
