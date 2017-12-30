using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using System;

namespace SpotFinder.Redux.Actions.CurrentPlace
{
    class GetPlaceByIdCompleteAction : IAction
    {
        public Place Place { get; private set; }
        public Status Status { get; private set; }
        public Exception Error { get; private set; }

        public GetPlaceByIdCompleteAction(Place place)
        {
            Place = place;
            Status = Status.Success;
        }

        public GetPlaceByIdCompleteAction(Exception error)
        {
            Error = error;
            Status = Status.Error;
        }
    }
}
