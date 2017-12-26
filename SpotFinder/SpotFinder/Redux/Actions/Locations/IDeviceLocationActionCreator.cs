using System;

namespace SpotFinder.Redux.Actions.Locations
{
    public interface IDeviceLocationActionCreator
    {
        StoreExtensions.AsyncActionCreator<ApplicationState> RequestDeviceLocation(TimeSpan timeSpanForGettingLocation);
    }
}
