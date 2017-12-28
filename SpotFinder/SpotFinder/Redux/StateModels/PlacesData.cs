﻿using SpotFinder.Models.Core;
using System;

namespace SpotFinder.Redux.StateModels
{
    public class PlacesData
    {
        public AsyncOperationState<Place, int> CurrentPlaceState { get; private set; }
        public PlacesListState PlacesListState { get; private set; }
        public Report Report { get; private set; }

        [Obsolete]
        public PlacesData()
        {

        }

        public PlacesData(
            AsyncOperationState<Place, int> currentPlaceState, 
            PlacesListState placesListState, 
            Report report
        )
        {
            CurrentPlaceState = currentPlaceState;
            PlacesListState = placesListState;
            Report = report;
        }
    }
}
