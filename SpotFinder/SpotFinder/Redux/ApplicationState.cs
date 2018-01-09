﻿using SpotFinder.Core.Enums;
using SpotFinder.Redux.StateModels;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive;

namespace SpotFinder.Redux
{
    public class ApplicationState
    {
        public IImmutableDictionary<PermissionName, AsyncOperationState<PermissionStatus, Unit>> PermissionsDictionary { get; private set; }
        public Stack<PageName> NavigationStack { get; private set; }
        public Settings Settings { get; private set; }
        public PlacesData PlacesData { get; private set; }
        public DeviceData DeviceData { get; private set; }
        public ErrorState Error { get; private set; }
        public UserState UserState { get; private set; }

        [Obsolete]
        public ApplicationState() { }

        public ApplicationState(
            IImmutableDictionary<PermissionName, AsyncOperationState<PermissionStatus, Unit>> permissionsDictionary, 
            Stack<PageName> navigationStack, 
            Settings settings, 
            PlacesData placesData, 
            DeviceData deviceData,
            ErrorState error,
            UserState userState
        )
        {
            PermissionsDictionary = permissionsDictionary;
            NavigationStack = navigationStack;
            Settings = settings;
            PlacesData = placesData;
            DeviceData = deviceData;
            Error = error;
            UserState = userState;
        }
    }
}
