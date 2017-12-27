using Redux;
using System;

namespace SpotFinder.Redux.Actions.PlacesList
{
    public class DownloadPlacesListByCriteriaErrorCompleteAction : IAction
    {
        public Exception Error { get; private set; }

        public DownloadPlacesListByCriteriaErrorCompleteAction(Exception exception)
        {
            Error = exception;
        }
    }
}
