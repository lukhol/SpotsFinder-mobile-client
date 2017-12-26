using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using SpotFinder.Models.Core;
using System;

namespace SpotFinder.Redux.Actions.Locations
{
    public class DeviceLocationActionCreator : IDeviceLocationActionCreator
    {
        public StoreExtensions.AsyncActionCreator<ApplicationState> RequestDeviceLocation(TimeSpan timeSpanForGettingLocation)
        {
            return async (dispatch, getState) =>
            {
                dispatch(new GetDeviceLocationStartAction());

                Position position = null;

                try
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;

                    //Last casched location:
                    //position = await locator.GetLastKnownLocationAsync();

                    if(!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                    {
                        var error = new Exception("Geolocator is not available or not enabled!");
                        dispatch(new GetDeviceLocationErrorCompleteAction(error));
                    }

                    position = await locator.GetPositionAsync(timeSpanForGettingLocation);
                    
                    if(position == null)
                    {
                        var error = new Exception("Cannpot get location!");
                        dispatch(new GetDeviceLocationErrorCompleteAction(error));
                    }

                    var location = new Location(position.Latitude, position.Longitude);

                    dispatch(new GetDeviceLocationSuccessCompleteAction(location));
                }
                catch (Exception e)
                {
                    dispatch(new GetDeviceLocationErrorCompleteAction(e));
                }
            };
        }
    }
}
