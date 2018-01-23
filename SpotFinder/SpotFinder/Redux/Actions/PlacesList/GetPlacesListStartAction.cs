using Redux;
using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux.Actions.PlacesList
{
    public class GetPlacesStartAction<T> : IAction
    {
        public T Value { get; private set; }
        public Status Status { get; private set; }

        public GetPlacesStartAction(T value)
        {
            Value = value;
            Status = Status.Getting;
        }
    }
}
