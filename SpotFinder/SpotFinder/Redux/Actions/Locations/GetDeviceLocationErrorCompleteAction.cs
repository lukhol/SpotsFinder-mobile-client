using Redux;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpotFinder.Redux.Actions.Locations
{
    class GetDeviceLocationErrorCompleteAction : IAction
    {
        public Exception Error { get; private set; }

        public GetDeviceLocationErrorCompleteAction(Exception error)
        {
            Error = error;
        }
    }
}
