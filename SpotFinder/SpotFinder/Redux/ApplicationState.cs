using System;
using System.Collections.Generic;
using SpotFinder.Redux.StateModels;

namespace SpotFinder.Redux
{
    public class ApplicationState
    {
        //Navigation section
        public Stack<PageName> NavigationStack { get; set; }

        //Settings section
        public Settings Settings { get; set; }

        //Data section
        public PlacesData PlacesData { get; set; }

        //Device information section
        public DeviceData DeviceData { get; set; }

        public ApplicationState()
        {
            PlacesData = new PlacesData();
            DeviceData = new DeviceData();
        }
    }
}
