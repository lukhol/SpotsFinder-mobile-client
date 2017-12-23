using SpotFinder.Models.Core;
using System.Collections.Generic;
using System;

namespace SpotFinder.Redux.StateModels
{
    public class PlacesData
    {
        public Criteria Criteria { get; set; }
        public Place ShowingPlace { get; set; }
        public List<Place> ListOfPlaces { get; set; }
        public Report Report { get; set; }
    }
}
