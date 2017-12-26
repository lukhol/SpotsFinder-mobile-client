using SpotFinder.Models.Core;
using System;

namespace SpotFinder.Redux.StateModels
{
    public class PlacesData
    {
        public Criteria Criteria { get; private set; }
        public CurrentPlaceState CurrentPlaceState { get; private set; }
        public PlacesListState PlacesListState { get; private set; }
        public Report Report { get; private set; }

        [Obsolete]
        public PlacesData()
        {

        }

        public PlacesData(Criteria criteria, CurrentPlaceState currentPlaceState, PlacesListState placesListState, Report report)
        {
            Criteria = criteria;
            CurrentPlaceState = currentPlaceState;
            PlacesListState = placesListState;
            Report = report;
        }
    }
}
