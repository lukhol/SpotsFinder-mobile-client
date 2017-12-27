using Redux;
using System;

namespace SpotFinder.Redux.Actions.CurrentPlace
{
    public class DownloadPlaceByIdErrorCompleteAction : IAction
    {
        public Exception Error { get; private set; }

        public DownloadPlaceByIdErrorCompleteAction(Exception error)
        {
            Error = error;
        }
    }
}
