using System;
using System.Collections.Generic;
using SpotFinder.Redux.StateModels;
using System.Collections.Immutable;
using SpotFinder.Core.Enums;
using SpotFinder.Models.Core;
using System.Reactive;

namespace SpotFinder.Redux
{
    public class ApplicationState
    {
        //Permissions:
        public IImmutableDictionary<PermissionName, AsyncOperationState<PermissionStatus, Unit>> PermissionsDictionary { get; set; }

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
            var currentPlaceState = new AsyncOperationState<Place, int>(
                Status.Empty, string.Empty, null, 0
            );
            var placesListState = new PlacesListState(null, Status.Unknown, null, new List<Place>());

            PlacesData = new PlacesData(currentPlaceState, placesListState, new Report());

            var location = new Location(0, 0);
            var locationState = new LocationState(Status.Empty, null, location);

            DeviceData = new DeviceData(locationState);

            var permissionDictionary = new Dictionary<PermissionName, AsyncOperationState<PermissionStatus, Unit>>();
            permissionDictionary.Add(PermissionName.Camera, 
                new AsyncOperationState<PermissionStatus, Unit>(Status.Unknown, string.Empty, PermissionStatus.Unknown, Unit.Default)
            );
            permissionDictionary.Add(PermissionName.Location,
                new AsyncOperationState<PermissionStatus, Unit>(Status.Unknown, string.Empty, PermissionStatus.Unknown, Unit.Default)    
            );
            permissionDictionary.Add(PermissionName.Storage, 
                new AsyncOperationState<PermissionStatus, Unit>(Status.Unknown, string.Empty, PermissionStatus.Unknown, Unit.Default)    
            );
            PermissionsDictionary = permissionDictionary.ToImmutableDictionary();
        }
    }
}
