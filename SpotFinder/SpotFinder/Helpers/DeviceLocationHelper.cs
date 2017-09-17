using Microsoft.Practices.ServiceLocation;
using Plugin.Geolocator;
using SpotFinder.Core;
using System;

namespace SpotFinder.Helpers
{
    public class DeviceLocationHelper : IDeviceLocationHelper
    {
        public async void GetLocationAndSaveInReportManagerOnStartAsync()
        {
            var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 10;

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
                if (position == null)
                {
                    return;
                }

                reportManager.Location = new Models.Core.Location();

                reportManager.Location.Latitude = position.Latitude;
                reportManager.Location.Longitude = position.Longitude;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("", e);
            }
        }
    }
}
