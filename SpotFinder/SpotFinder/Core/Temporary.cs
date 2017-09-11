using Microsoft.Practices.ServiceLocation;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Core
{
    public static class Temporary
    {
        public static async void GetLocationAndSaveInReportManagerOnStartAsync()
        {
            var reportManager = ServiceLocator.Current.GetInstance<ReportManager>();

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 10;

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(3));
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
                Temporary.GetLocationAndSaveInReportManagerOnStartAsync();
            }
        }
    }
}
