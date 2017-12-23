using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using SpotFinder.Redux.Actions;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {
        public ICommand SearchSpotsNearCommand => new Command(SearchSpotsNear);

        private void SearchSpotsNear()
        {
            var deviceLocation = App.AppStore.GetState().DeviceData?.Location;

            if (deviceLocation == null)
                return;

            var criteria = new Criteria();
            criteria.Distance = 25;
            criteria.Type = new List<PlaceType>
            {
                PlaceType.DIY, PlaceType.Skatepark, PlaceType.Skatespot
            };
            criteria.Location = new CityLocation
            {
                Longitude = deviceLocation.Longitude,
                Latitude = deviceLocation.Latitude
            };

            if (criteria == null)
                return;

            App.AppStore.Dispatch(new ClearSpotsListAction());
            App.AppStore.Dispatch(new ReplaceCriteriaAction(criteria));
        }
    }
}
