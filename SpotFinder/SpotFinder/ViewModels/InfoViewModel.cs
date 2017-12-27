using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using SpotFinder.Redux;
using SpotFinder.Redux.Actions;
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

            appStore.Dispatch(new SetCriteriaAction(criteria));
        }
    }
}
