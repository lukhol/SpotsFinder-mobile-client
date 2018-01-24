using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using System;

namespace SpotFinder.Redux.StateModels
{
    public class Report
    {
        public Place Place { get; private set; }
        public Location Location { get; private set; }
        public ReportType ReportType { get; private set; }

        [Obsolete]
        public Report()
        {

        }

        public Report(Place place, Location location, ReportType reportType)
        {
            Place = place;
            Location = location;
            ReportType = reportType;
        }
    }
}
