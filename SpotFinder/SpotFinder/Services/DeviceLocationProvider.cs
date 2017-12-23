using Plugin.Geolocator;
using SpotFinder.Redux.Actions;
using System;
using Xamarin.Forms;

namespace SpotFinder.Services
{
    public class DeviceLocationProvider : IDeviceLocationProvider
    {
        public bool IsAcquiring { get; private set; }
        public bool IsLoopActivated { get; private set; }

        public void RequestDeviceLocationLoopAsync()
        {
            if(IsLoopActivated == false)
            {
                IsLoopActivated = true;

                GetLocationFromGPSAsync();

                Device.StartTimer(TimeSpan.FromMinutes(1), () =>
                {
                    GetLocationFromGPSAsync();
                    return true;
                });
            }
        }

        public void RequestDeviceLocationOnesAsync()
        {
            GetLocationFromGPSAsync();
        }

        public async void RequestDeviceLocationForReportAsync()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 10;

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(8));
                if (position != null)
                {
                    App.AppStore.Dispatch(new UpdateLocationInReportAction(position.Longitude, position.Latitude));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception during getting location from device.", e);
            }
        }

        private async void GetLocationFromGPSAsync()
        {
            if (IsAcquiring == true)
                return;

            IsAcquiring = true;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 10;

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(8));
                if (position != null)
                {
                    App.AppStore.Dispatch(new UpdateDeviceLocationAction(position.Longitude, position.Latitude));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception during getting location from device.", e);
            }
            IsAcquiring = false;
        }
    }
}
