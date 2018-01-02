using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using SpotFinder.Redux;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {
        public ICommand SearchSpotsNearCommand => new Command(SearchSpotsNear);

        public InfoViewModel(IStore<ApplicationState> appStore) : base(appStore)
        {

        }

        private void SearchSpotsNear()
        {
            var deviceLocation = appStore.GetState().DeviceData?.LocationState.Value;

            if (deviceLocation == null)
                return;

            var distance = 25;
            var types = new List<PlaceType>
            {
                PlaceType.DIY, PlaceType.Skatepark, PlaceType.Skatespot
            };
            var location = new CityLocation
            {
                Longitude = deviceLocation.Longitude,
                Latitude = deviceLocation.Latitude
            };

            var criteria = new Criteria(types, location, distance);

            if (criteria == null)
                return;

            //TODO: Start downloading spots list.
        }
    }
}
