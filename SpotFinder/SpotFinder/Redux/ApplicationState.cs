using System;
using System.Collections.Generic;
using SpotFinder.Redux.StateModels;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
