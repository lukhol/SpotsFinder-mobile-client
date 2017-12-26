using System;
using System.Collections.Generic;
using SpotFinder.Redux.StateModels;
using System.Collections.Immutable;
using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;

namespace SpotFinder.Redux
{
    public class ApplicationState
    {
        //Permissions:
        public IImmutableDictionary<PermissionName, Permission> PermissionsDictionary { get; set; }

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
            //TODO: All of this initialization should be inside bootstrapper or DIContainer!
            var currentPlaceState = new CurrentPlaceState(0, Status.Unknown, null, null);
            var placesListState = new PlacesListState(null, Status.Unknown, null, new List<Place>());

            PlacesData = new PlacesData(null, currentPlaceState, placesListState, new Report());

            var location = new Location(0, 0);
            var locationState = new LocationState(Status.NotStartedYet, null, location);

            DeviceData = new DeviceData(locationState);
            var permissionDictionary = new Dictionary<PermissionName, Permission>();
            permissionDictionary.Add(PermissionName.Camera, new Permission());
            permissionDictionary.Add(PermissionName.Location, new Permission());
            permissionDictionary.Add(PermissionName.Storage, new Permission());
            PermissionsDictionary = permissionDictionary.ToImmutableDictionary();
        }
    }
}
